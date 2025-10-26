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
using System.Linq;
using DustInTheWind.Bani.Adapters.DataAccess.Infrastructure;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.DataAccess;

internal class IssuerRepository : IIssuerRepository
{
    private readonly BaniDbContext dbContext;
    private readonly ChangeTracker changeTracker;

    public IssuerRepository(BaniDbContext dbContext, ChangeTracker changeTracker)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.changeTracker = changeTracker ?? throw new ArgumentNullException(nameof(changeTracker));
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

    public Issuer Get(string id)
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
        changeTracker.TrackAdd(issuer);
    }

    public void Update(Issuer issuer)
    {
        ArgumentNullException.ThrowIfNull(issuer);

        if (string.IsNullOrEmpty(issuer.Id))
            throw new ArgumentException("Issuer Id cannot be null or empty.", nameof(issuer));

        // Find existing issuer in memory
        Issuer existingIssuer = dbContext.Issuers.FirstOrDefault(x => x.Id == issuer.Id);
        if (existingIssuer == null)
            throw new InvalidOperationException($"Issuer with Id '{issuer.Id}' not found.");

        // Update the existing issuer properties
        existingIssuer.Name = issuer.Name;
        existingIssuer.Location = issuer.Location;
        existingIssuer.Comments = issuer.Comments;
        // Note: We don't update Emissions here as they might be managed separately

        // Track the change
        changeTracker.TrackUpdate(existingIssuer);
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

        // Find and remove from in-memory collection
        Issuer existingIssuer = dbContext.Issuers.FirstOrDefault(x => x.Id == id);

        if (existingIssuer == null)
            throw new InvalidOperationException($"Issuer with Id '{id}' not found.");

        dbContext.Issuers.Remove(existingIssuer);

        changeTracker.TrackRemove(existingIssuer);
    }
}