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

using System.Collections;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

/// <summary>
/// An observable collection that automatically tracks additions, removals, and modifications of entities
/// using snapshot-based change detection.
/// </summary>
/// <typeparam name="T">The type of entities in the collection.</typeparam>
public class ObservableEntityCollection<T> : ICollection<T>
    where T : class, IEntity
{
    private readonly List<T> items = [];
    private readonly SnapshotChangeTracker<T> changeTracker;

    public int Count => items.Count;

    public bool IsReadOnly => false;

    public ObservableEntityCollection()
    {
        changeTracker = new SnapshotChangeTracker<T>();
    }

    /// <summary>
    /// Initializes the collection with a set of entities from the data source.
    /// These entities are tracked as unchanged initially.
    /// </summary>
    /// <param name="initialItems">The entities loaded from the data source.</param>
    public void InitializeWith(IEnumerable<T> initialItems)
    {
        items.Clear();
        changeTracker.Clear();

        if (initialItems != null)
        {
            foreach (T item in initialItems)
            {
                items.Add(item);
                changeTracker.TrackEntity(item);
            }
        }
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        items.Add(item);

        EntityEntry<T> existingEntry = changeTracker.GetEntry(item.Id);

        if (existingEntry?.State == EntityState.Deleted)
            existingEntry.State = EntityState.Unchanged;
        else if (existingEntry == null)
            changeTracker.TrackAdd(item);

        // If existingEntry exists and is not deleted, it means the entity is already in the collection
        // and this might be a duplicate add - we'll allow it but won't change tracking
    }

    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return RemoveById(item.Id);
    }

    public bool RemoveById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;

        T item = items.FirstOrDefault(x => x.Id == id);
        if (item == null)
            return false;

        bool isRemoved = items.Remove(item);

        if (isRemoved)
        {
            EntityEntry<T> entry = changeTracker.GetEntry(id);
            if (entry != null)
            {
                if (entry.State == EntityState.Added)
                    changeTracker.StopTracking(id);
                else
                    changeTracker.TrackRemove(id);
            }
        }

        return isRemoved;
    }

    public void Clear()
    {
        List<T> trackedEntities = changeTracker.GetAllTrackedEntities()
            .ToList();

        foreach (T item in trackedEntities)
        {
            EntityEntry<T> entry = changeTracker.GetEntry(item.Id);

            if (entry?.State == EntityState.Added)
                changeTracker.StopTracking(item.Id);
            else if (entry != null)
                changeTracker.TrackRemove(item.Id);
        }

        items.Clear();
    }

    public bool Contains(T item)
    {
        return items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        items.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Commits all changes by accepting them in the change tracker and updating the baseline.
    /// </summary>
    public void CommitChanges()
    {
        changeTracker.AcceptChanges();
    }

    /// <summary>
    /// Manually triggers change detection on all tracked entities.
    /// This compares current entity values with their snapshots.
    /// Usually called before getting changes or saving.
    /// </summary>
    public void DetectChanges()
    {
        changeTracker.DetectChanges();
    }

    /// <summary>
    /// Gets all pending changes that need to be persisted.
    /// Automatically triggers change detection before returning results.
    /// </summary>
    /// <returns>A collection of change entries representing the pending changes.</returns>
    public IEnumerable<ChangeEntry<T>> GetChanges()
    {
        return changeTracker.GetChanges();
    }

    /// <summary>
    /// Gets whether the collection has any pending changes.
    /// </summary>
    public bool HasChanges => changeTracker.HasChanges;

    /// <summary>
    /// Gets detailed information about a specific entity's tracking state.
    /// </summary>
    /// <param name="entityId">The ID of the entity to get entry information for.</param>
    /// <returns>The entity entry containing state and change information, or null if not tracked.</returns>
    public EntityEntry<T> GetEntry(string entityId)
    {
        return changeTracker.GetEntry(entityId);
    }

    /// <summary>
    /// Gets detailed information about a specific entity's tracking state.
    /// </summary>
    /// <param name="entity">The entity to get entry information for.</param>
    /// <returns>The entity entry containing state and change information, or null if not tracked.</returns>
    public EntityEntry<T> GetEntry(T entity)
    {
        return changeTracker.GetEntry(entity);
    }

    /// <summary>
    /// Legacy method for backward compatibility.
    /// In snapshot-based tracking, this is equivalent to calling DetectChanges() first.
    /// </summary>
    /// <param name="entity">The entity to track for updates.</param>
    public void TrackUpdate(T entity)
    {
        if (changeTracker.GetEntry(entity) == null)
            changeTracker.TrackEntity(entity);

        changeTracker.DetectChanges();
    }
}