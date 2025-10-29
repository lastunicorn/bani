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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DustInTheWind.Bani.Avalonia.Application.PresentIssuers;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.Issuers;

public class IssuerViewModel
{
    public IssuerInfo IssuerInfo { get; }

    public string Text { get; }

    public ObservableCollection<EmissionViewModel> Emissions { get; }

    public IssuerViewModel(IssuerInfo issuerInfo)
    {
        IssuerInfo = issuerInfo ?? throw new ArgumentNullException(nameof(issuerInfo));
        Text = IssuerInfo?.Name;

        IEnumerable<EmissionViewModel> emmissions = issuerInfo.Emissions?
            .Select(x => new EmissionViewModel(x))
            ?? [];

        Emissions = new ObservableCollection<EmissionViewModel>(emmissions);
    }
}