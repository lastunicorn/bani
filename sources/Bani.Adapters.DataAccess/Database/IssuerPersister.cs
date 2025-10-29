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

using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;
using DustInTheWind.JFileDb;

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

public class IssuerPersister : IEntityPersister<Issuer>
{
    public async Task PersistAddedAsync(Issuer entity, CancellationToken cancellationToken = default)
    {
        JsonFile<JIssuer> issuerFile = new(entity.Id)
        {
            Data = entity.ToJsonEntity()
        };

        await issuerFile.SaveAsync(cancellationToken);
    }

    public async Task PersistModifiedAsync(Issuer entity, CancellationToken cancellationToken = default)
    {
        JsonFile<JIssuer> issuerFile = new(entity.Id)
        {
            Data = entity.ToJsonEntity()
        };

        await issuerFile.SaveAsync(cancellationToken);
    }

    public async Task PersistDeletedAsync(Issuer entity, CancellationToken cancellationToken = default)
    {
        JsonFile<JIssuer> issuerFile = new(entity.Id)
        {
            Data = entity.ToJsonEntity()
        };

        if (issuerFile.Exists())
            issuerFile.Delete();

        // For consistency, we'll keep this async even though Delete is synchronous
        await Task.CompletedTask;
    }
}