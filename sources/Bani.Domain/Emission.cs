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

using System.Collections.Generic;

namespace DustInTheWind.Bani.Domain;

public class Emission
{
    public string Location { get; set; }

    public string Name { get; set; }

    public int? StartYear { get; set; }

    public int? EndYear { get; set; }

    public string Comments { get; set; }

    public List<Artifact> Artifacts { get; } = [];

    public bool IsBetween(int? startYear, int? endYear)
    {
        if (startYear == null && endYear == null)
            return true;

        if (startYear == null)
            return StartYear == null || endYear.Value >= StartYear.Value;

        if (endYear == null)
            return EndYear == null || startYear.Value <= EndYear.Value;

        return ContainsDate(endYear.Value) ||
               ((EndYear == null || endYear.Value > EndYear.Value) && StartYear.Value <= EndYear);
    }

    public bool ContainsDate(int year)
    {
        return (StartYear == null || year >= StartYear.Value) &&
               (EndYear == null || year <= EndYear.Value);
    }
}