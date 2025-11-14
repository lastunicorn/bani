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
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;

namespace DustInTheWind.Bani.Adapters.DataAccess;

internal class EmissionRepository : IEmissionRepository
{
    private readonly BaniDbContext dbContext;

    public EmissionRepository(BaniDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Emission GetById(string id)
    {
        return dbContext.Issuers
            .SelectMany(x => x.Emissions)
            .Where(x => x.Name == id)
            .FirstOrDefault();
    }
}
