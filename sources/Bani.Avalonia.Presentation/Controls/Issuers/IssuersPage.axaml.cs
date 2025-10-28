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

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DustInTheWind.Bani.Avalonia.Presentation.Controls.Main;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Issuers;

public partial class IssuersPage : UserControl
{
    public IssuersPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InputElement_OnTapped(object sender, TappedEventArgs e)
    {
        if (DataContext is IssuersPageViewModel mainWindowViewModel)
        {
            SelectIssueCommand selectIssueCommand = mainWindowViewModel.SelectIssueCommand;

            object selectedItem = sender is ListBox listBox
                ? listBox.SelectedItem
                : null;

            if (selectIssueCommand?.CanExecute(selectedItem) == true)
                selectIssueCommand.Execute(selectedItem);
        }
    }

    private async void CommentsTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        if (DataContext is IssuersPageViewModel viewModel)
        {
            await viewModel.SaveIssuerComments();
        }
    }
}