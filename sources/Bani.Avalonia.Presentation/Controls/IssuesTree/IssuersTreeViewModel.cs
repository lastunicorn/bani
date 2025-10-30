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

using System.Collections.ObjectModel;
using Avalonia.Controls;
using DustInTheWind.Bani.Avalonia.Application.PresentIssuesTree;
using DustInTheWind.Bani.Avalonia.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.IssuesTree;

public class IssuersTreeViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private object selectedItem;

    public ObservableCollection<IssuerTreeNodeViewModel> Issues { get; } = [];

    public object SelectedItem
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            OnPropertyChanged();
        }
    }

    public IssuersTreeViewModel(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try
        {
            PresentIssuersTreeRequest request = new();
            PresentIssuersTreeResponse response = await mediator.Send(request);

            IEnumerable<IssuerTreeNodeViewModel> issueViewModels = response.Issuers
                .Select(x => new IssuerTreeNodeViewModel(x));

            foreach (IssuerTreeNodeViewModel issueViewModel in issueViewModels)
                Issues.Add(issueViewModel);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            System.Diagnostics.Debug.WriteLine($"Failed to initialize IssuersTreeViewModel: {ex.Message}");
        }
    }
}