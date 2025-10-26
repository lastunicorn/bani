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
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Ports.DataAccess;

public interface IIssuerRepository
{
    /// <summary>
    /// Gets all issuers from the repository.
    /// </summary>
    IEnumerable<Issuer> GetAll();

    /// <summary>
    /// Gets issuers by name using case-insensitive search.
    /// </summary>
    IEnumerable<Issuer> GetByName(string name);

    /// <summary>
    /// Gets a specific issuer by its unique identifier.
    /// </summary>
    Issuer Get(string id);

    /// <summary>
    /// Adds a new issuer to the repository.
    /// Changes are not persisted until SaveChanges is called on the Unit of Work.
    /// </summary>
    void Add(Issuer issuer);

    /// <summary>
    /// Updates an existing issuer in the repository.
    /// Changes are not persisted until SaveChanges is called on the Unit of Work.
    /// </summary>
    void Update(Issuer issuer);

    /// <summary>
    /// Removes an issuer from the repository.
    /// Changes are not persisted until SaveChanges is called on the Unit of Work.
    /// </summary>
    void Remove(Issuer issuer);

    /// <summary>
    /// Removes an issuer by its identifier from the repository.
    /// Changes are not persisted until SaveChanges is called on the Unit of Work.
    /// </summary>
    void Remove(string id);
}