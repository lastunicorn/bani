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

using System.Windows.Input;
using DustInTheWind.Bani.Avalonia.Application.SelectIssuer;
using MediatR;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Main;

public class SelectIssueCommand : ICommand
{
    private readonly IMediator mediator;
    public event EventHandler CanExecuteChanged;

    public SelectIssueCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _ = SendRequest(parameter);
    }

    private async Task SendRequest(object parameter)
    {
        string issuerId = null;

        SelectIssuerRequest request = new()
        {
            IssuerId = issuerId
        };

        await mediator.Send(request);
    }

    protected virtual void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}