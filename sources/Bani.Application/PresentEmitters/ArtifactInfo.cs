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
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Application.PresentEmitters
{
    public class ArtifactInfo
    {
        public string DisplayName { get; }

        public DateTime? IssueDate { get; }

        public int? Year { get; }

        public int InstanceCount { get; }

        public ArtifactType ArtifactType { get; }

        public ArtifactInfo(Artifact artifact)
        {
            DisplayName = artifact.DisplayName;
            IssueDate = artifact.IssueDate;
            Year = artifact.Year;
            InstanceCount = artifact.InstanceCount;
            ArtifactType = CalculateArtifactType(artifact);
        }

        private static ArtifactType CalculateArtifactType(Artifact artifact)
        {
            return artifact switch
            {
                Coin => ArtifactType.Coin,
                Banknote => ArtifactType.Banknote,
                _ => ArtifactType.Unknown
            };
        }
    }
}