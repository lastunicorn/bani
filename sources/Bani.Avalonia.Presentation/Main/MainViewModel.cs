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
using System.Reflection;
using DustInTheWind.Bani.Avalonia.Application.SelectIssuer;
using DustInTheWind.Bani.Avalonia.Presentation.Issuers;
using DustInTheWind.Bani.Avalonia.Presentation.PageTitle;
using DustInTheWind.Bani.Avalonia.Presentation.ViewModels;
using DustInTheWind.Bani.Infrastructure;

namespace DustInTheWind.Bani.Avalonia.Presentation.Main;

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

    public PageTitleViewModel PageTitleViewModel { get; } = new();

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

    public IssuersPageViewModel IssuersPageViewModel { get; }

    public MainViewModel(EventBus eventBus, SelectIssueCommand selectIssueCommand, IssuersPageViewModel issuersPageViewModel)
    {
        SelectIssueCommand = selectIssueCommand;
        IssuersPageViewModel = issuersPageViewModel;

        eventBus.Subscribe<IssuerChangedEvent>(HandleIssuerChanged);

        PageTitleViewModel.Title = "Issuer";
        PageTitleViewModel.Description = "Please select the issuer from the list.";

        BreadCrumbs = "> ";
    }

    private void HandleIssuerChanged(IssuerChangedEvent ev)
    {
        BreadCrumbs = "> " + ev.Issuer?.Name;

        // todo: display the catalogs page
    }
}