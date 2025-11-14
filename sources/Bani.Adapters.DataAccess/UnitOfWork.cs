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
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.Adapters.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly BaniDbContext dbContext;
    private IIssuerRepository issuerRepository;
    private IEmissionRepository emissionRepository;

    public IIssuerRepository IssuerRepository => issuerRepository ??= new IssuerRepository(dbContext);

    public IEmissionRepository EmissionRepository => emissionRepository ??= new EmissionRepository(dbContext);

    public UnitOfWork(string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        dbContext = new BaniDbContext(connectionString);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return dbContext.CommitChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // In a real scenario, you might want to rollback in-memory changes here
            throw new InvalidOperationException("Failed to save changes to data store.", ex);
        }
    }
}