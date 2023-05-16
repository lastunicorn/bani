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
using DustInTheWind.Bani.Cli.Application.PresentIssuers;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.ConsoleTools.Controls;

namespace DustInTheWind.Bani.Cli.Presentation;

internal class PresentIssuersView : ViewBase<IssuerCommand>
{
    public override void Display(IssuerCommand command)
    {
        foreach (IssuerInfo issuerInfo in command.Issuers)
            DisplayIssuer(issuerInfo);
    }

    private void DisplayIssuer(IssuerInfo issuerInfo)
    {
        WriteTitle(issuerInfo.Name);

        foreach (EmissionInfo emissionInfo in issuerInfo.Emissions)
        {
            DisplayEmission(emissionInfo);
            Console.WriteLine();
        }
    }

    private static void DisplayEmission(EmissionInfo emissionInfo)
    {
        EmissionControl emissionControl = new()
        {
            EmissionInfo = emissionInfo
        };
        emissionControl.Display();
    }
}