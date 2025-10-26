// Bani
// Copyright (C) 2022 Dust in the Wind
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DustInTheWind.Bani.Avalonia.Application.PresentIssuers;
using DustInTheWind.Bani.Avalonia.Presentation.Controls.Main;
using DustInTheWind.Bani.Avalonia.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Issuers;

public class IssuersPageViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private IssuerViewModel selectedIssuer;
    private string issuerComments;
    private ObservableCollection<EmissionViewModel> emissions;

    public ObservableCollection<IssuerViewModel> Issuers { get; } = [];

    public IssuerViewModel SelectedIssuer
    {
        get => selectedIssuer;
        set
        {
            selectedIssuer = value;
            OnPropertyChanged();

            IssuerComments = value?.IssuerInfo?.Comments;
            Emissions = value?.Emissions ?? [];
        }
    }

    public string IssuerComments
    {
        get => issuerComments;
        set
        {
            issuerComments = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<EmissionViewModel> Emissions
    {
        get => emissions;
        set
        {
            emissions = value;
            OnPropertyChanged();
        }
    }

    public SelectIssueCommand SelectIssueCommand { get; }

    public IssuersPageViewModel(IMediator mediator, SelectIssueCommand selectIssueCommand)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        SelectIssueCommand = selectIssueCommand;

        _ = Initialize();
    }

    private async Task Initialize()
    {
        PresentIssuersRequest request = new();
        PresentIssuersResponse response = await mediator.Send(request);

        IEnumerable<IssuerViewModel> issuerViewModels = response.Issuers
            .Select(x => new IssuerViewModel(x));

        foreach (IssuerViewModel issuerViewModel in issuerViewModels)
            Issuers.Add(issuerViewModel);
    }
}