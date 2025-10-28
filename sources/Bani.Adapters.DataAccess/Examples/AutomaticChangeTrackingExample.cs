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
using DustInTheWind.Bani.Adapters.DataAccess.Database;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.Adapters.DataAccess.Examples;

/// <summary>
/// Demonstrates how automatic change tracking works without manual TrackAdd/TrackRemove calls.
/// </summary>
public class AutomaticChangeTrackingExample
{
    private readonly IUnitOfWork unitOfWork;

    public AutomaticChangeTrackingExample(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void DemonstrateAutomaticChangeTracking()
    {
        // 1. Add a new issuer - automatically tracked as Added
        var newIssuer = new Issuer
        {
            Id = "/path/to/new/issuer.json",
            Name = "Test Issuer",
            Location = "Test Location",
            Comments = "This is a test issuer"
        };

        // This automatically calls changeTracker.TrackAdd(newIssuer) internally
        unitOfWork.IssuerRepository.Add(newIssuer);

        // 2. Update an existing issuer - manually tracked as Modified (since we're changing properties)
        var existingIssuer = unitOfWork.IssuerRepository.Get("/some/existing/issuer.json");
        if (existingIssuer != null)
        {
            var updatedIssuer = new Issuer
            {
                Id = existingIssuer.Id,
                Name = "Updated Name",
                Location = existingIssuer.Location,
                Comments = "Updated comments"
            };

            // This calls changeTracker.TrackUpdate(existingIssuer) internally
            unitOfWork.IssuerRepository.Update(updatedIssuer);
        }

        // 3. Remove an issuer - automatically tracked as Deleted
        // This automatically calls changeTracker.TrackRemove(issuerId) internally
        unitOfWork.IssuerRepository.Remove("/path/to/issuer/to/remove.json");

        // 4. Save all changes - this will persist all tracked changes
        unitOfWork.SaveChanges();

        Console.WriteLine("All changes have been automatically tracked and saved!");
    }

    public void DemonstrateDirectCollectionManipulation()
    {
        // This approach also works and automatically tracks changes:
        var unitOfWorkImpl = (UnitOfWork)unitOfWork;
        var dbContext = GetDbContextFromUnitOfWork(unitOfWorkImpl);

        var newIssuer = new Issuer
        {
            Id = "/direct/path/new/issuer.json",
            Name = "Direct Add Issuer",
            Location = "Direct Location"
        };

        // Adding directly to the collection also triggers automatic tracking
        dbContext.Issuers.Add(newIssuer);

        // Removing directly from the collection also triggers automatic tracking
        var issuerToRemove = dbContext.Issuers.FirstOrDefault(i => i.Name == "Some Issuer");
        if (issuerToRemove != null)
        {
            dbContext.Issuers.Remove(issuerToRemove);
        }

        // Save changes
        unitOfWork.SaveChanges();

        Console.WriteLine("Direct collection manipulation also automatically tracked changes!");
    }

    private BaniDbContext GetDbContextFromUnitOfWork(UnitOfWork unitOfWork)
    {
        // In a real scenario, you might expose the DbContext through a property
        // This is just for demonstration purposes
        throw new NotImplementedException("This would require exposing DbContext from UnitOfWork");
    }
}