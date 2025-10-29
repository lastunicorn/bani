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
/// A simple change tracker that manually tracks entity state changes.
/// Unlike the SnapshotChangeTracker, this doesn't automatically detect property changes.
/// </summary>
/// <typeparam name="TEntity">The entity type that implements IEntity</typeparam>
public class ChangeTracker<TEntity>
    where TEntity : class, IEntity
{
    private readonly Dictionary<string, ChangeEntry<TEntity>> changes = [];

    /// <summary>
    /// Gets whether there are any tracked changes.
    /// </summary>
    public bool HasChanges => changes.Count > 0;

    /// <summary>
    /// Tracks an entity as added.
    /// </summary>
    /// <param name="entity">The entity to track as added.</param>
    public void TrackAdd(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (string.IsNullOrEmpty(entity.Id))
            throw new ArgumentException("Entity must have an Id", nameof(entity));

        changes[entity.Id] = new ChangeEntry<TEntity>
        {
            Entity = entity,
            State = EntityState.Added
        };
    }

    /// <summary>
    /// Tracks an entity as modified.
    /// </summary>
    /// <param name="entity">The entity to track as modified.</param>
    public void TrackUpdate(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (string.IsNullOrEmpty(entity.Id))
            throw new ArgumentException("Entity must have an Id", nameof(entity));

        bool changeItemExists = changes.ContainsKey(entity.Id);

        if (changeItemExists)
            return;

        changes[entity.Id] = new ChangeEntry<TEntity>
        {
            Entity = entity,
            State = EntityState.Modified
        };
    }

    /// <summary>
    /// Tracks an entity as removed.
    /// </summary>
    /// <param name="entity">The entity to track as removed.</param>
    public void TrackRemove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        TrackRemove(entity.Id);
    }

    /// <summary>
    /// Tracks an entity as removed by its ID.
    /// </summary>
    /// <param name="entityId">The ID of the entity to track as removed.</param>
    public void TrackRemove(string entityId)
    {
        if (string.IsNullOrEmpty(entityId))
            throw new ArgumentException("Entity Id cannot be null or empty", nameof(entityId));

        bool changeItemExists = changes.TryGetValue(entityId, out ChangeEntry<TEntity> existingEntry);

        if (changeItemExists)
        {
            switch (existingEntry.State)
            {
                case EntityState.Added:
                    changes.Remove(entityId);
                    return;

                case EntityState.Modified:
                    changes[entityId] = new ChangeEntry<TEntity>
                    {
                        Entity = existingEntry?.Entity,
                        State = EntityState.Deleted
                    };
                    break;

                case EntityState.Deleted:
                    return;

                default:
                    break;
            }
        }

        changes[entityId] = new ChangeEntry<TEntity>
        {
            Entity = existingEntry?.Entity,
            State = EntityState.Deleted
        };
    }

    /// <summary>
    /// Gets all tracked changes.
    /// </summary>
    /// <returns>A collection of change entries.</returns>
    public IEnumerable<ChangeEntry<TEntity>> GetChanges()
    {
        return changes.Values;
    }

    /// <summary>
    /// Clears all tracked changes.
    /// </summary>
    public void Clear()
    {
        changes.Clear();
    }
}