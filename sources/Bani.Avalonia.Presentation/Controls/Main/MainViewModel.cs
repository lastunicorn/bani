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

using System.Reflection;
using DustInTheWind.Bani.Avalonia.Application.SelectIssuer;
using DustInTheWind.Bani.Avalonia.Presentation.Controls.Details;
using DustInTheWind.Bani.Avalonia.Presentation.Controls.IssuesTree;
using DustInTheWind.Bani.Avalonia.Presentation.Infrastructure;
using DustInTheWind.Bani.Infrastructure;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Main;

public class MainViewModel : ViewModelBase
{
    private string breadCrumbs;

    public string WindowTitle
    {
        get
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;

            return $"Bani {version.ToString(3)}";
        }
    }

    public string BreadCrumbs
    {
        get => breadCrumbs;
        set
        {
            breadCrumbs = value;
            OnPropertyChanged();
        }
    }

    public SelectIssueCommand SelectIssueCommand { get; }

    public DetailsPageViewModel DetailsPageViewModel { get; }

    public IssuersTreeViewModel IssuersTreeViewModel { get; }

    public MainViewModel(EventBus eventBus, SelectIssueCommand selectIssueCommand,
        DetailsPageViewModel detailsPageViewModel, IssuersTreeViewModel issuesTreeViewModel)
    {
        SelectIssueCommand = selectIssueCommand;
        DetailsPageViewModel = detailsPageViewModel;
        IssuersTreeViewModel = issuesTreeViewModel;

        eventBus.Subscribe<IssuerChangedEvent>(HandleIssuerChanged);

        BreadCrumbs = "> ";
    }

    private void HandleIssuerChanged(IssuerChangedEvent ev)
    {
        BreadCrumbs = "> " + ev.Issuer?.Name;

        // todo: display the catalogs page
    }
}