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

using Autofac;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DustInTheWind.Bani.Avalonia.Presentation.ViewModels;
using DustInTheWind.Bani.Avalonia.Presentation.Views;

namespace DustInTheWind.Bani.Avalonia;

public partial class App : global::Avalonia.Application
{
    private IContainer container;

    public override void Initialize()
    {
        container = SetupServices.BuildContainer();

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = container?.Resolve<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}