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

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

/// <summary>
/// A change tracker that uses snapshot-based change detection.
/// Similar to Entity Framework's approach, it compares current entity values 
/// with stored snapshots to detect modifications.
/// </summary>
/// <typeparam name="TEntity">The entity type that implements IEntity</typeparam>
public class SnapshotChangeTracker<TEntity>
    where TEntity : class, IEntity
{
    private readonly Dictionary<string, EntityEntry<TEntity>> entries = [];

    /// <summary>
    /// Gets whether there are any tracked changes.
    /// </summary>
    public bool HasChanges
    {
        get
        {
            DetectChanges();
            return entries.Values.Any(e => e.State != EntityState.Unchanged);
        }
    }

    /// <summary>
    /// Starts tracking an entity as added.
    /// </summary>
    /// <param name="entity">The entity to track as added.</param>
    public void TrackAdd(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (string.IsNullOrEmpty(entity.Id))
            throw new ArgumentException("Entity must have an Id", nameof(entity));

        entries[entity.Id] = new EntityEntry<TEntity>(entity, EntityState.Added);
    }

    /// <summary>
    /// Starts tracking an entity for changes. Takes an initial snapshot.
    /// </summary>
    /// <param name="entity">The entity to track for changes.</param>
    public void TrackEntity(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (string.IsNullOrEmpty(entity.Id))
            throw new ArgumentException("Entity must have an Id", nameof(entity));

        if (entries.ContainsKey(entity.Id))
            return;

        entries[entity.Id] = new EntityEntry<TEntity>(entity, EntityState.Unchanged);
    }

    /// <summary>
    /// Marks an entity as deleted.
    /// </summary>
    /// <param name="entity">The entity to mark as deleted.</param>
    public void TrackRemove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        TrackRemove(entity.Id);
    }

    /// <summary>
    /// Marks an entity as deleted by its ID.
    /// </summary>
    /// <param name="entityId">The ID of the entity to mark as deleted.</param>
    public void TrackRemove(string entityId)
    {
        if (string.IsNullOrEmpty(entityId))
            throw new ArgumentException("Entity Id cannot be null or empty", nameof(entityId));

        if (entries.TryGetValue(entityId, out EntityEntry<TEntity> existingEntry))
        {
            switch (existingEntry.State)
            {
                case EntityState.Added:
                    entries.Remove(entityId);
                    return;

                case EntityState.Modified:
                case EntityState.Unchanged:
                    existingEntry.State = EntityState.Deleted;
                    break;

                case EntityState.Deleted:
                    return;

                default:
                    throw new InvalidOperationException($"Unknown entity state: {existingEntry.State}");
            }
        }
        else
        {
            throw new InvalidOperationException($"Cannot delete untracked entity with ID: {entityId}");
        }
    }

    /// <summary>
    /// Forces change detection on all tracked entities.
    /// Compares current entity values with their snapshots.
    /// </summary>
    public void DetectChanges()
    {
        foreach (EntityEntry<TEntity> entry in entries.Values)
            entry.DetectChanges();
    }

    /// <summary>
    /// Gets all tracked changes.
    /// </summary>
    /// <returns>A collection of change entries for entities that have been modified.</returns>
    public IEnumerable<ChangeEntry<TEntity>> GetChanges()
    {
        DetectChanges();

        return entries.Values
            .Where(x => x.State != EntityState.Unchanged)
            .Select(x => new ChangeEntry<TEntity>
            {
                Entity = x.Entity,
                State = x.State
            })
            .ToList();
    }

    /// <summary>
    /// Gets the entity entry for a specific entity ID.
    /// </summary>
    /// <param name="entityId">The entity ID.</param>
    /// <returns>The entity entry, or null if not found.</returns>
    public EntityEntry<TEntity> GetEntry(string entityId)
    {
        entries.TryGetValue(entityId, out EntityEntry<TEntity> entry);
        return entry;
    }

    /// <summary>
    /// Gets the entity entry for a specific entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The entity entry, or null if not found.</returns>
    public EntityEntry<TEntity> GetEntry(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return GetEntry(entity.Id);
    }

    /// <summary>
    /// Accepts all changes and resets the change tracker.
    /// Takes new snapshots of all unchanged/modified entities and removes deleted entities.
    /// </summary>
    public void AcceptChanges()
    {
        List<string> entriesToRemove = [];

        foreach (KeyValuePair<string, EntityEntry<TEntity>> kvp in entries)
        {
            EntityEntry<TEntity> entry = kvp.Value;

            switch (entry.State)
            {
                case EntityState.Deleted:
                    entriesToRemove.Add(kvp.Key);
                    break;

                case EntityState.Added:
                case EntityState.Modified:
                case EntityState.Unchanged:
                    entry.AcceptChanges();
                    break;

                default:
                    throw new InvalidOperationException($"Unknown entity state: {entry.State}");
            }
        }

        foreach (string id in entriesToRemove)
            entries.Remove(id);
    }

    /// <summary>
    /// Clears all tracked entities and their change information.
    /// </summary>
    public void Clear()
    {
        entries.Clear();
    }

    /// <summary>
    /// Gets all tracked entities regardless of their state.
    /// </summary>
    /// <returns>All tracked entities.</returns>
    public IEnumerable<TEntity> GetAllTrackedEntities()
    {
        return entries.Values.Select(x => x.Entity);
    }

    /// <summary>
    /// Stops tracking a specific entity.
    /// </summary>
    /// <param name="entityId">The ID of the entity to stop tracking.</param>
    /// <returns>True if the entity was being tracked and is now removed, false otherwise.</returns>
    public bool StopTracking(string entityId)
    {
        return entries.Remove(entityId);
    }

    /// <summary>
    /// Stops tracking a specific entity.
    /// </summary>
    /// <param name="entity">The entity to stop tracking.</param>
    /// <returns>True if the entity was being tracked and is now removed, false otherwise.</returns>
    public bool StopTracking(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return StopTracking(entity.Id);
    }
}