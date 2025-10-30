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

namespace DustInTheWind.Bani.Avalonia.Application.PresentIssuesTree;

public class EmissionTreeNodeInfo
{
    public string Name { get; }

    public string DisplayText { get; }

    public TreeNodeType NodeType { get; }

    public int? StartYear { get; }

    public int? EndYear { get; }

    public EmissionTreeNodeInfo(Emission emission)
    {
        ArgumentNullException.ThrowIfNull(emission);

        Name = emission.Name;
        StartYear = emission.StartYear;
        EndYear = emission.EndYear;
        NodeType = TreeNodeType.Emission;

        DisplayText = CreateDisplayText(emission.Name, emission.StartYear, emission.EndYear);
    }

    private static string CreateDisplayText(string name, int? startYear, int? endYear)
    {
        string yearRange = CreateYearRangeText(startYear, endYear);
        return string.IsNullOrEmpty(yearRange) ? name : $"{name} ({yearRange})";
    }

    private static string CreateYearRangeText(int? startYear, int? endYear)
    {
        if (startYear == null && endYear == null)
            return string.Empty;

        if (startYear == null)
            return $"-{endYear}";

        if (endYear == null)
            return $"{startYear}-";

        if (startYear == endYear)
            return startYear.ToString();

        return $"{startYear}-{endYear}";
    }
}