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

using System.Collections.Generic;
using System.Linq;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Cli.Application.PresentIssuers
{
    public class EmissionInfo
    {
        public string Name { get; }

        public int? StartYear { get; }

        public int? EndYear { get; }

        public List<ArtifactInfo> Artifacts { get; }

        public EmissionInfo(Emission emission)
        {
            Name = emission.Name;
            StartYear = emission.StartYear;
            EndYear = emission.EndYear;
            Artifacts = emission.Artifacts
                .OrderBy(x => x.GetType().FullName)
                .ThenBy(x => x.Value)
                .ThenBy(x => x.Year)
                .Select(x => new ArtifactInfo(x))
                .ToList();
        }
    }
}