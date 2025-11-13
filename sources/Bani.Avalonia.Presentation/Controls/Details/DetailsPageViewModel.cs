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
    private Issuer currentIssuer;
    private string comments;

    public Issuer CurrentIssuer
    {
        get => currentIssuer;
        private set
        {
            currentIssuer = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IssuerName));
            OnPropertyChanged(nameof(IsVisible));
        }
    }

    public string IssuerName => CurrentIssuer?.Name ?? string.Empty;

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

    public bool IsVisible => CurrentIssuer != null;

    public DetailsPageViewModel(EventBus eventBus, RequestBus requestBus)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<IssuerChangedEvent>(OnIssuerChanged);
    }

    private void OnIssuerChanged(IssuerChangedEvent issuerChangedEvent)
    {
        CurrentIssuer = issuerChangedEvent.Issuer;
        Comments = CurrentIssuer?.Comments ?? string.Empty;
    }

    private async Task UpdateCommentsAsync()
    {
        if (CurrentIssuer == null)
            return;

        try
        {
            UpdateIssuerCommentsRequest request = new()
            {
                IssuerId = CurrentIssuer.Id,
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