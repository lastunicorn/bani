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

public class BaniDbContext
{
    private readonly string connectionString;
    private readonly EntityPersisterFactory persisterFactory;

    public ObservableEntityCollection<Issuer> Issuers { get; }

    public BaniDbContext(string connectionString)
    {
        this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        persisterFactory = new EntityPersisterFactory();

        RegisterPersisters();

        Issuers = [];

        LoadIssuers();
    }

    private void RegisterPersisters()
    {
        persisterFactory.Register(new IssuerPersister());

        // When you add more entity types, register their persisters here:
        // persisterFactory.Register<Emission>(new EmissionPersister());
        // persisterFactory.Register<Artifact>(new ArtifactPersister());
    }

    private void LoadIssuers()
    {
        IssuerCrawler issuerCrawler = new();
        IEnumerable<Issuer> issuers = issuerCrawler.Crawl(connectionString);
        Issuers.InitializeWith(issuers);
    }

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        await PersistChangesAsync(Issuers, cancellationToken);
        Issuers.CommitChanges();
    }

    private async Task PersistChangesAsync<TEntity>(ObservableEntityCollection<TEntity> collection, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity
    {
        IEntityPersister<TEntity> persister = persisterFactory.GetPersister<TEntity>();

        foreach (ChangeEntry<TEntity> change in collection.GetChanges())
        {
            cancellationToken.ThrowIfCancellationRequested();

            switch (change.State)
            {
                case EntityState.Added:
                    await persister.PersistAddedAsync(change.Entity, cancellationToken);
                    break;

                case EntityState.Modified:
                    await persister.PersistModifiedAsync(change.Entity, cancellationToken);
                    break;

                case EntityState.Deleted:
                    await persister.PersistDeletedAsync(change.Entity, cancellationToken);
                    break;
            }
        }
    }
}