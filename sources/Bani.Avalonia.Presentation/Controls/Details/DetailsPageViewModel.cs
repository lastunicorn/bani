// Bani
// Copyright (C) 2022-2025 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using DustInTheWind.Bani.Avalonia.Application.SelectIssuer;
using DustInTheWind.Bani.Avalonia.Application.UpdateIssuerComments;
using DustInTheWind.Bani.Avalonia.Presentation.Infrastructure;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Infrastructure;
using DustInTheWind.RequestR;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Details;

public class DetailsPageViewModel : ViewModelBase
{
    private readonly RequestBus requestBus;
    private object currentItem;
    private string title;
    private string comments;

    public object CurrentItem
    {
        get => currentItem;
        private set
        {
            currentItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(IsVisible));
        }
    }

    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }

    public string Comments
    {
        get => comments;
        set
        {
            if (comments != value)
            {
                comments = value;
                OnPropertyChanged();

                _ = UpdateCommentsAsync();
            }
        }
    }

    public bool IsVisible => CurrentItem != null;

    public DetailsPageViewModel(EventBus eventBus, RequestBus requestBus)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<CurrentItemChangedEvent>(OnIssuerChanged);
    }

    private void OnIssuerChanged(CurrentItemChangedEvent issuerChangedEvent)
    {
        CurrentItem = issuerChangedEvent.CurrentItem;
        Title = issuerChangedEvent.CurrentItem switch
        {
            Issuer issuer => issuer.Name,
            Emission emission => emission.Name,
            _ => string.Empty
        };
        Comments = issuerChangedEvent.CurrentItem switch
        {
            Issuer issuer => issuer.Comments,
            Emission emission => emission.Comments,
            _ => string.Empty
        };
    }

    private async Task UpdateCommentsAsync()
    {
        if (CurrentItem == null)
            return;

        try
        {
            UpdateIssuerCommentsRequest request = new()
            {
                IssuerId = CurrentItem switch
                {
                    Issuer issuer => issuer.Id,
                    Emission emission => emission.Name,
                    _ => string.Empty
                },
                Comments = Comments
            };

            await requestBus.ProcessAsync(request);
        }
        catch (Exception ex)
        {
            // Log error but don't throw to avoid breaking UI
            System.Diagnostics.Debug.WriteLine($"Error updating issuer comments: {ex.Message}");
        }
    }
}