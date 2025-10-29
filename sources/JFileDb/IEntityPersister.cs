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
/// Defines the contract for persisting entities to storage.
/// This is a generic interface that can be implemented for any storage mechanism.
/// </summary>
/// <typeparam name="TEntity">The type of entity to persist.</typeparam>
public interface IEntityPersister<in TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Persists a newly added entity to storage.
    /// </summary>
    /// <param name="entity">The entity to persist.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A task representing the async operation.</returns>
    Task PersistAddedAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists modifications to an existing entity in storage.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A task representing the async operation.</returns>
    Task PersistModifiedAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an entity from storage.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A task representing the async operation.</returns>
    Task PersistDeletedAsync(TEntity entity, CancellationToken cancellationToken = default);
}