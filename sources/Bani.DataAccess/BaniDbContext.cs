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
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    public class BaniDbContext
    {
        private readonly string connectionString;

        public List<Issuer> Issuers { get; } = new();

        public BaniDbContext(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            LoadIssuers();
        }

        private void LoadIssuers()
        {
            Issuers.Clear();

            IssuerCrawler issuerCrawler = new();
            IEnumerable<Issuer> issuers = issuerCrawler.Crawl(connectionString);
            Issuers.AddRange(issuers);
        }
    }
}