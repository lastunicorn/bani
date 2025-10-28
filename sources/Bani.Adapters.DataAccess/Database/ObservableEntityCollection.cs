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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

/// <summary>
/// An observable collection that automatically tracks additions and removals of entities.
/// </summary>
/// <typeparam name="T">The type of entities in the collection.</typeparam>
public class ObservableEntityCollection<T> : ICollection<T> where T : class, IEntity
{
    private readonly List<T> items = [];
    private readonly HashSet<T> originalItems = [];
    private readonly ChangeTracker<T> changeTracker;

    public int Count => items.Count;

    public bool IsReadOnly => false;

    public ObservableEntityCollection()
    {
        changeTracker = new ChangeTracker<T>();
    }

    /// <summary>
    /// Initializes the collection with existing items without tracking them as changes.
    /// This should be called when loading data from the data source.
    /// </summary>
    public void InitializeWith(IEnumerable<T> initialItems)
    {
        items.Clear();
        originalItems.Clear();

        if (initialItems != null)
        {
            items.AddRange(initialItems);

            foreach (T item in items)
                originalItems.Add(item);
        }
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        items.Add(item);

        // Only track as addition if this item wasn't in the original collection
        if (!originalItems.Contains(item))
            changeTracker.TrackAdd(item);
    }

    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        bool removed = items.Remove(item);

        if (removed)
        {
            // Only track as removal if this item was in the original collection
            if (originalItems.Contains(item))
                changeTracker.TrackRemove(item);
        }

        return removed;
    }

    public bool RemoveById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;

        T item = items.FirstOrDefault(x => x.Id == id);
        if (item != null)
            return Remove(item);

        return false;
    }

    public void Clear()
    {
        foreach (T item in items.Where(originalItems.Contains))
            changeTracker.TrackRemove(item);

        items.Clear();
    }

    public bool Contains(T item) => items.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CommitChanges()
    {
        originalItems.Clear();

        foreach (T item in items)
            originalItems.Add(item);

        changeTracker.Clear();
    }

    public T FirstOrDefault(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        return items.Where(predicate);
    }

    public bool Any(Func<T, bool> predicate)
    {
        return items.Any(predicate);
    }

    public void TrackUpdate(T entity)
    {
        changeTracker.TrackUpdate(entity);
    }

    public IEnumerable<ChangeEntry<T>> GetChanges()
    {
        return changeTracker.GetChanges();
    }
}