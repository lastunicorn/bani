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

namespace DustInTheWind.JFileDb;

public abstract class DbContext
{
    private readonly EntityPersisterFactory entityPersisterFactory;

    protected DbContext()
    {
        entityPersisterFactory = new EntityPersisterFactory();
        RegisterPersisters(entityPersisterFactory);
    }

    protected abstract void RegisterPersisters(EntityPersisterFactory entityPersisterFactory);

    /// <summary>
    /// Commits changes for all ObservableEntityCollection properties using reflection.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A task representing the async operation.</returns>
    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<PropertyInfo> properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(ObservableEntityCollection<>));

        foreach (PropertyInfo property in properties)
        {
            Type entityType = property.PropertyType.GetGenericArguments()[0];
            object collection = property.GetValue(this);

            if (collection != null)
            {
                await PersistChangesForCollectionAsync(collection, entityType, cancellationToken);

                MethodInfo commitChangesMethod = property.PropertyType.GetMethod("CommitChanges");
                commitChangesMethod?.Invoke(collection, null);
            }
        }
    }

    private Task PersistChangesForCollectionAsync(object collection, Type entityType, CancellationToken cancellationToken)
    {
        MethodInfo method = typeof(DbContext).GetMethod("PersistChangesAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo genericMethod = method.MakeGenericMethod(entityType);

        return (Task)genericMethod.Invoke(this, new object[] { collection, cancellationToken });
    }

    /// <remarks>
    /// This method is called using Reflection to persist changes for a specific ObservableEntityCollection.
    /// </remarks>
    private async Task PersistChangesAsync<TEntity>(ObservableEntityCollection<TEntity> collection, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        IEntityPersister<TEntity> entityPersister = entityPersisterFactory.GetPersister<TEntity>();

        foreach (ChangeEntry<TEntity> change in collection.GetChanges())
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (change.State)
            {
                case EntityState.Added:
                    await entityPersister.PersistAddedAsync(change.Entity, cancellationToken);
                    break;

                case EntityState.Modified:
                    await entityPersister.PersistModifiedAsync(change.Entity, cancellationToken);
                    break;

                case EntityState.Deleted:
                    await entityPersister.PersistDeletedAsync(change.Entity, cancellationToken);
                    break;
            }
        }
    }
}