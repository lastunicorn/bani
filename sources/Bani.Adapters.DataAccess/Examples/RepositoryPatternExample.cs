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
using System.Threading.Tasks;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.Adapters.DataAccess.Examples;

/// <summary>
/// Example demonstrating proper usage of the Repository and Unit of Work patterns.
/// </summary>
public class RepositoryPatternExample
{
    private readonly IUnitOfWork unitOfWork;

    public RepositoryPatternExample(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Example: Update issuer comments properly using the patterns
    /// </summary>
    public async Task UpdateIssuerCommentsExample(string issuerId, string newComments)
    {
        // Get the issuer using the repository
        Issuer? issuer = unitOfWork.IssuerRepository.Get(issuerId);

        if (issuer == null)
            throw new InvalidOperationException($"Issuer with ID '{issuerId}' not found.");

        // Modify the entity
        issuer.Comments = newComments;

        // Mark as updated in the repository (this only tracks the change, doesn't persist yet)
        unitOfWork.IssuerRepository.Update(issuer);

        // Persist all changes atomically
        await unitOfWork.SaveChangesAsync();

        // The issuer comments have been successfully saved to the JSON file
    }

    /// <summary>
    /// Example: Add a new issuer using the patterns
    /// </summary>
    public async Task AddNewIssuerExample(string filePath, string name, string comments)
    {
        var newIssuer = new Issuer
        {
            Id = filePath, // In this system, the file path serves as the ID
            Name = name,
            Comments = comments
        };

        // Add to repository (this only tracks the change, doesn't persist yet)
        unitOfWork.IssuerRepository.Add(newIssuer);

        // Persist all changes atomically
        await unitOfWork.SaveChangesAsync();

        // The new issuer has been successfully created as a JSON file
    }

    /// <summary>
    /// Example: Remove an issuer using the patterns
    /// </summary>
    public async Task RemoveIssuerExample(string issuerId)
    {
        // Remove from repository (this only tracks the change, doesn't persist yet)
        unitOfWork.IssuerRepository.Remove(issuerId);

        // Persist all changes atomically
        await unitOfWork.SaveChangesAsync();

        // The issuer JSON file has been successfully deleted
    }

    /// <summary>
    /// Example: Multiple operations in a single transaction
    /// </summary>
    public async Task MultipleOperationsExample()
    {
        // Perform multiple operations
        var newIssuer = new Issuer
        {
            Id = "/path/to/new/issuer.json",
            Name = "New Issuer",
            Comments = "This is a new issuer"
        };

        unitOfWork.IssuerRepository.Add(newIssuer);

        // Update existing issuer
        Issuer? existingIssuer = unitOfWork.IssuerRepository.Get("/path/to/existing/issuer.json");
        if (existingIssuer != null)
        {
            existingIssuer.Comments = "Updated comments";
            unitOfWork.IssuerRepository.Update(existingIssuer);
        }

        // Remove another issuer
        unitOfWork.IssuerRepository.Remove("/path/to/remove/issuer.json");

        // All changes are persisted atomically in a single call
        await unitOfWork.SaveChangesAsync();

        // If any operation fails, none of the changes will be persisted
    }
}