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
using DustInTheWind.Bani.Ports.StateAccess;

namespace DustInTheWind.Bani.Adapters.StateAccess;

public class ApplicationState : IApplicationState
{
    private Issuer currentIssuer;
    private Emission currentEmission;

    public ItemType CurrentItemType { get; private set; }

    public Issuer CurrentIssuer
    {
        get => CurrentItemType == ItemType.Issuer ? currentIssuer : null;
        set
        {
            currentIssuer = value;
            CurrentItemType = ItemType.Issuer;
        }
    }

    public Emission CurrentEmission
    {
        get => CurrentItemType == ItemType.Emission ? currentEmission : null;
        set
        {
            currentEmission = value;
            CurrentItemType = ItemType.Emission;
        }
    }

    public void RemoveCurrent()
    {
        CurrentItemType = ItemType.None;
    }
}