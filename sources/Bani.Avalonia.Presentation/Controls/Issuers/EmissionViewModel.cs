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
using DustInTheWind.Bani.Avalonia.Application.PresentIssuers;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Issuers;

public class EmissionViewModel
{
    public EmissionInfo EmissionInfo { get; }

    public string Name { get; }

    public string YearRange { get; }

    public string DisplayText { get; }

    public EmissionViewModel(EmissionInfo emissionInfo)
    {
        EmissionInfo = emissionInfo ?? throw new ArgumentNullException(nameof(emissionInfo));
        Name = emissionInfo.Name;

        YearRange = CreateYearRangeText(emissionInfo.StartYear, emissionInfo.EndYear);
        DisplayText = string.IsNullOrEmpty(YearRange) ? Name : $"{Name} ({YearRange})";
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