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
using System.Collections.Generic;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

public class EntityPersisterFactory
{
    private readonly Dictionary<Type, object> persisters = [];

    public void Register<TEntity>(IEntityPersister<TEntity> persister)
        where TEntity : class, IEntity
    {
        persisters[typeof(TEntity)] = persister;
    }

    public IEntityPersister<TEntity> GetPersister<TEntity>()
        where TEntity : class, IEntity
    {
        if (!persisters.TryGetValue(typeof(TEntity), out var persister))
            throw new InvalidOperationException($"No persister registered for entity type {typeof(TEntity).Name}");

        return (IEntityPersister<TEntity>)persister;
    }

    public bool HasPersister<TEntity>()
        where TEntity : class, IEntity
    {
        return persisters.ContainsKey(typeof(TEntity));
    }

    public bool HasPersister(Type entityType)
    {
        return persisters.ContainsKey(entityType);
    }
}