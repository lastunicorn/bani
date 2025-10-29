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

namespace DustInTheWind.JFileDb;

/// <summary>
/// Represents a change to an entity, including the entity itself and its state.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public class ChangeEntry<T>
{
    /// <summary>
    /// Gets or sets the entity that has been changed.
    /// </summary>
    public T Entity { get; set; }

    /// <summary>
    /// Gets or sets the state of the entity (Added, Modified, Deleted, etc.).
    /// </summary>
    public EntityState State { get; set; }
}