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

using System.Reflection;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

/// <summary>
/// Represents the state and change tracking information for an entity.
/// </summary>
/// <typeparam name="T">The entity type that implements IEntity</typeparam>
public class EntityEntry<T> where T : class, IEntity
{
    private readonly Dictionary<string, object> originalValues = [];
    private readonly Dictionary<string, PropertyInfo> propertyInfoCache = [];

    public T Entity { get; }

    public EntityState State { get; set; }

    public IReadOnlyDictionary<string, object> OriginalValues => originalValues;

    public EntityEntry(T entity, EntityState state = EntityState.Unchanged)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        State = state;

        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanRead && x.CanWrite)
            .ToArray();

        foreach (PropertyInfo property in properties)
            propertyInfoCache[property.Name] = property;

        if (state != EntityState.Added)
            TakeSnapshot();
    }

    /// <summary>
    /// Takes a snapshot of the current entity property values.
    /// </summary>
    public void TakeSnapshot()
    {
        originalValues.Clear();

        foreach (KeyValuePair<string, PropertyInfo> kvp in propertyInfoCache)
        {
            object value = kvp.Value.GetValue(Entity);
            originalValues[kvp.Key] = CloneValue(value);
        }
    }

    /// <summary>
    /// Detects if the entity has been modified by comparing current values with the snapshot.
    /// </summary>
    /// <returns>True if the entity has been modified, false otherwise.</returns>
    public bool DetectChanges()
    {
        if (State == EntityState.Added || State == EntityState.Deleted)
            return true;

        foreach (KeyValuePair<string, PropertyInfo> kvp in propertyInfoCache)
        {
            if (IsPropertyModified(kvp.Key))
            {
                if (State == EntityState.Unchanged)
                    State = EntityState.Modified;

                return true;
            }
        }

        if (State == EntityState.Modified)
            State = EntityState.Unchanged;

        return false;
    }

    /// <summary>
    /// Checks if a specific property has been modified.
    /// </summary>
    /// <param name="propertyName">The name of the property to check.</param>
    /// <returns>True if the property has been modified, false otherwise.</returns>
    public bool IsPropertyModified(string propertyName)
    {
        if (!propertyInfoCache.TryGetValue(propertyName, out var propertyInfo))
            return false;

        if (!originalValues.TryGetValue(propertyName, out var originalValue))
            return false;

        var currentValue = propertyInfo.GetValue(Entity);
        return !ValuesEqual(originalValue, currentValue);
    }

    /// <summary>
    /// Gets the list of modified property names.
    /// </summary>
    /// <returns>A collection of property names that have been modified.</returns>
    public IEnumerable<string> GetModifiedProperties()
    {
        return propertyInfoCache.Keys.Where(IsPropertyModified);
    }

    /// <summary>
    /// Accepts all changes by taking a new snapshot of the current state.
    /// </summary>
    public void AcceptChanges()
    {
        if (State == EntityState.Deleted)
            return;

        TakeSnapshot();
        State = EntityState.Unchanged;
    }

    /// <summary>
    /// Compares two values for equality, handling null values and different types appropriately.
    /// </summary>
    private static bool ValuesEqual(object value1, object value2)
    {
        // Handle null cases
        if (value1 == null && value2 == null)
            return true;

        if (value1 == null || value2 == null)
            return false;

        if (value1.GetType() != value2.GetType())
            return false;

        return value1.Equals(value2);
    }

    /// <summary>
    /// Creates a copy of a value for snapshot purposes.
    /// For value types and immutable types like string, returns the value directly.
    /// For other reference types, this could be extended to create deep copies if needed.
    /// </summary>
    private static object CloneValue(object value)
    {
        if (value == null)
            return null;

        Type type = value.GetType();

        if (type.IsValueType || type == typeof(string))
            return value;

        // todo: For other reference types, you might want to implement deep cloning
        // For now, we'll store the reference and rely on change detection
        // being called before the object is mutated externally
        return value;
    }
}