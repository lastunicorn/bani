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
/// Represents the state of an entity in the change tracking system.
/// </summary>
public enum EntityState
{
    /// <summary>
    /// The entity has not been modified since it was loaded or last saved.
    /// </summary>
    Unchanged,

    /// <summary>
    /// The entity is new and will be inserted when changes are saved.
    /// </summary>
    Added,

    /// <summary>
    /// The entity has been modified and will be updated when changes are saved.
    /// </summary>
    Modified,

    /// <summary>
    /// The entity has been marked for deletion and will be deleted when changes are saved.
    /// </summary>
    Deleted
}