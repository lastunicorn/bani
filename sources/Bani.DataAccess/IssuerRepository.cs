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
using DustInTheWind.Bani.DataAccess.Port;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    public class IssuerRepository : IIssuerRepository
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
            return dbContext.Issuers
                .Where(x => x.Name?.Contains(name, StringComparison.InvariantCultureIgnoreCase) ?? false);
        }

        public Issuer Get(string id)
        {
            return dbContext.Issuers
                .FirstOrDefault(x => x.Id == id);
        }
    }
}