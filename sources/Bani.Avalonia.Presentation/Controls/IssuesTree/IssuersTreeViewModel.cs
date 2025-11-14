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
using DustInTheWind.Bani.Avalonia.Application.PresentIssuesTree;
using DustInTheWind.Bani.Avalonia.Application.SelectIssuer;
using DustInTheWind.Bani.Avalonia.Presentation.Infrastructure;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.StateAccess;
using DustInTheWind.RequestR;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.IssuesTree;

public class IssuersTreeViewModel : ViewModelBase
{
    private readonly RequestBus requestBus;
    private object selectedItem;

    public ObservableCollection<IssuerTreeNodeViewModel> Issues { get; } = [];

    public object SelectedItem
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            OnPropertyChanged();

            _ = HandleSelectionChanged(value);
        }
    }

    public IssuersTreeViewModel(RequestBus requestBus)
    {
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        PresentIssuersTreeRequest request = new();
        PresentIssuersTreeResponse response = await requestBus.ProcessAsync<PresentIssuersTreeRequest, PresentIssuersTreeResponse>(request);

        IEnumerable<IssuerTreeNodeViewModel> issueViewModels = response.Issuers
            .Select(x => new IssuerTreeNodeViewModel(x));

        foreach (IssuerTreeNodeViewModel issueViewModel in issueViewModels)
            Issues.Add(issueViewModel);
    }

    private async Task HandleSelectionChanged(object selectedItem)
    {
        SelectIssuerRequest request = new();

        if (selectedItem is IssuerTreeNodeViewModel issuerNode)
        {
            request.ItemId = issuerNode.Id;
            request.ItemType = ItemType.Issuer;

        }
        else if (selectedItem is EmissionTreeNodeViewModel emissionNode)
        {
            request.ItemId = emissionNode.Name;
            request.ItemType = ItemType.Emission;
        }

        await requestBus.ProcessAsync(request);
    }
}