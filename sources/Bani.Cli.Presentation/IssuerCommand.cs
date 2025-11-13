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

using DustInTheWind.Bani.Cli.Application.PresentIssuers;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.RequestR;

namespace DustInTheWind.Bani.Cli.Presentation;

[NamedCommand("issuer", Description = "Displays the list of issuers.")]
public class IssuerCommand : ICommand
{
    private readonly RequestBus requestBus;

    [NamedParameter("name", ShortName = 'n', IsOptional = true)]
    public string IssuerName { get; set; }

    [NamedParameter("start-year", ShortName = 's', IsOptional = true)]
    public int? StartYear { get; set; }

    [NamedParameter("end-year", ShortName = 'e', IsOptional = true)]
    public int? EndYear { get; set; }
    
    public List<IssuerInfo> Issuers { get; private set; }

    public IssuerCommand(RequestBus requestBus)
    {
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
    }

    public async Task Execute()
    {
        PresentIssuersRequest request = new()
        {
            IssuerName = IssuerName,
            StartYear = StartYear,
            EndYear = EndYear
        };
        PresentIssuersResponse response = await requestBus.ProcessAsync<PresentIssuersRequest, PresentIssuersResponse>(request);

        Issuers = response.Issuers;
    }
}