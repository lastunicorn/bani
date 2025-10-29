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

using DustInTheWind.Bani.Adapters.DataAccess.Database;
using DustInTheWind.Bani.Adapters.DataAccess.Helpers;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.Adapters.DataAccess;

internal class IssuerRepository : IIssuerRepository
{
    private readonly BaniDbContext dbContext;

    public IssuerRepository(BaniDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IEnumerable<Issuer> GetAll()
    {
        return dbContext.Issuers;
    }

    public IEnumerable<Issuer> GetByName(string name)
    {
        if (name.IsNullOrEmpty())
            return [];

        return dbContext.Issuers
            .Where(x => x.Name?.Contains(name, StringComparison.InvariantCultureIgnoreCase) ?? false);
    }

    public Issuer GetById(string id)
    {
        if (id.IsNullOrEmpty())
            return null;

        return dbContext.Issuers
            .FirstOrDefault(x => x.Id == id);
    }

    public void Add(Issuer issuer)
    {
        ArgumentNullException.ThrowIfNull(issuer);

        if (issuer.Id.IsNullOrEmpty())
            throw new ArgumentException("Issuer Id cannot be null or empty.", nameof(issuer));

        if (dbContext.Issuers.Any(x => x.Id == issuer.Id))
            throw new InvalidOperationException($"An issuer with Id '{issuer.Id}' already exists.");

        dbContext.Issuers.Add(issuer);
    }

    public void Remove(Issuer issuer)
    {
        ArgumentNullException.ThrowIfNull(issuer);
        Remove(issuer.Id);
    }

    public void Remove(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        if (id.Length == 0)
            throw new ArgumentException("Issuer Id cannot be empty.", nameof(id));

        bool removed = dbContext.Issuers.RemoveById(id);

        if (!removed)
            throw new InvalidOperationException($"Issuer with Id '{id}' not found.");
    }
}