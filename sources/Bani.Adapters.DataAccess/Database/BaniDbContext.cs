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
using DustInTheWind.JFileDb;

namespace DustInTheWind.Bani.Adapters.DataAccess.Database;

public class BaniDbContext : DbContext
{
    private readonly string connectionString;

    public ObservableEntityCollection<Issuer> Issuers { get; }

    public BaniDbContext(string connectionString)
    {
        this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        Issuers = [];

        LoadIssuers();
    }

    protected override void RegisterPersisters(EntityPersisterFactory entityPersisterFactory)
    {
        entityPersisterFactory.Register(new IssuerPersister());
        //entityPersisterFactory.Register<Emission>(new EmissionPersister());
        //entityPersisterFactory.Register<Artifact>(new ArtifactPersister());
    }

    private void LoadIssuers()
    {
        IssuerCrawler issuerCrawler = new();
        IEnumerable<Issuer> issuers = issuerCrawler.Crawl(connectionString);
        Issuers.InitializeWith(issuers);
    }
}