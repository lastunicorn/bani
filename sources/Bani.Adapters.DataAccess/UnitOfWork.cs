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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.Bani.Adapters.DataAccess.Infrastructure;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly BaniDbContext dbContext;
    private readonly ChangeTracker changeTracker;
    private IIssuerRepository issuerRepository;
    private bool disposed = false;

    public IIssuerRepository IssuerRepository => issuerRepository ??= new IssuerRepository(dbContext, changeTracker);

    public UnitOfWork(BaniDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.changeTracker = new ChangeTracker();
    }

    public void SaveChanges()
    {
        try
        {
            PersistChanges();
            changeTracker.Clear();
        }
        catch (Exception ex)
        {
            // In a real scenario, you might want to rollback in-memory changes here
            throw new InvalidOperationException("Failed to save changes to data store.", ex);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await PersistChangesAsync(cancellationToken);
            changeTracker.Clear();
        }
        catch (Exception ex)
        {
            // In a real scenario, you might want to rollback in-memory changes here
            throw new InvalidOperationException("Failed to save changes to data store.", ex);
        }
    }

    private void PersistChanges()
    {
        foreach (ChangeEntry<Issuer> change in changeTracker.GetIssuerChanges())
        {
            switch (change.State)
            {
                case EntityState.Added:
                    PersistAddedIssuer(change.Entity);
                    break;

                case EntityState.Modified:
                    PersistModifiedIssuer(change.Entity);
                    break;

                case EntityState.Deleted:
                    PersistDeletedIssuer(change.Entity);
                    break;
            }
        }
    }

    private async Task PersistChangesAsync(CancellationToken cancellationToken)
    {
        foreach (ChangeEntry<Issuer> change in changeTracker.GetIssuerChanges())
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (change.State)
            {
                case EntityState.Added:
                    await PersistAddedIssuerAsync(change.Entity, cancellationToken);
                    break;

                case EntityState.Modified:
                    await PersistModifiedIssuerAsync(change.Entity, cancellationToken);
                    break;

                case EntityState.Deleted:
                    await PersistDeletedIssuerAsync(change.Entity, cancellationToken);
                    break;
            }
        }
    }

    private static void PersistAddedIssuer(Issuer issuer)
    {
        // For new issuers, we would need to create a new file
        // This is a simplified implementation
        IssuerFile issuerFile = new(issuer.Id)
        {
            Issuer = issuer.ToJsonEntity()
        };

        issuerFile.Save();
    }

    private static async Task PersistAddedIssuerAsync(Issuer issuer, CancellationToken cancellationToken)
    {
        // Async version - for now, just call the sync version
        // In a real implementation, you might use async file operations
        await Task.Run(() => PersistAddedIssuer(issuer), cancellationToken);
    }

    private static void PersistModifiedIssuer(Issuer issuer)
    {
        IssuerFile issuerFile = new(issuer.Id)
        {
            Issuer = issuer.ToJsonEntity()
        };

        issuerFile.Save();
    }

    private static async Task PersistModifiedIssuerAsync(Issuer issuer, CancellationToken cancellationToken)
    {
        // Async version - for now, just call the sync version
        // In a real implementation, you might use async file operations
        await Task.Run(() => PersistModifiedIssuer(issuer), cancellationToken);
    }

    private static void PersistDeletedIssuer(Issuer issuer)
    {
        if (File.Exists(issuer.Id))
            File.Delete(issuer.Id);
    }

    private static async Task PersistDeletedIssuerAsync(Issuer issuer, CancellationToken cancellationToken)
    {
        // Async version - for now, just call the sync version
        // In a real implementation, you might use async file operations
        await Task.Run(() => PersistDeletedIssuer(issuer), cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            // If there are unsaved changes, you might want to warn or auto-save
            if (changeTracker.HasChanges)
                Debug.WriteLine("Warning: UnitOfWork disposed with unsaved changes.");

            // Dispose any disposable resources if needed
            disposed = true;
        }
    }
}