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

using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Avalonia.Application.PresentIssuers;

public class IssuerInfo
{
    public string Id { get; set; }

    public string Name { get; }

    public string Comments { get; set; }

    public List<EmissionInfo> Emissions { get; }

    public IssuerInfo(Issuer issuer)
    {
        if (issuer == null) throw new ArgumentNullException(nameof(issuer));

        Id = issuer.Id;
        Name = issuer.Name;
        Comments = issuer.Comments;
        Emissions = issuer.Emissions
            .OrderBy(x => x.StartYear)
            .Select(x => new EmissionInfo(x))
            .ToList();
    }
}