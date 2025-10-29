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
/// Factory for managing and retrieving entity persisters.
/// Allows registration of different persisters for different entity types.
/// </summary>
public class EntityPersisterFactory
{
    private readonly Dictionary<Type, object> persisters = [];

    /// <summary>
    /// Registers a persister for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="persister">The persister instance.</param>
    public void Register<TEntity>(IEntityPersister<TEntity> persister)
        where TEntity : class, IEntity
    {
        persisters[typeof(TEntity)] = persister;
    }

    /// <summary>
    /// Gets the registered persister for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The persister instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no persister is registered for the entity type.</exception>
    public IEntityPersister<TEntity> GetPersister<TEntity>()
        where TEntity : class, IEntity
    {
        if (!persisters.TryGetValue(typeof(TEntity), out object persister))
            throw new InvalidOperationException($"No persister registered for entity type {typeof(TEntity).Name}");

        return (IEntityPersister<TEntity>)persister;
    }

    /// <summary>
    /// Checks if a persister is registered for a specific entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>True if a persister is registered, false otherwise.</returns>
    public bool HasPersister<TEntity>()
        where TEntity : class, IEntity
    {
        return persisters.ContainsKey(typeof(TEntity));
    }

    /// <summary>
    /// Checks if a persister is registered for a specific entity type.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>True if a persister is registered, false otherwise.</returns>
    public bool HasPersister(Type entityType)
    {
        return persisters.ContainsKey(entityType);
    }
}