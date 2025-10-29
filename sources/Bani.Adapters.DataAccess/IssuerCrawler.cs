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

using System.Collections.Generic;
using System.IO;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess;

internal class IssuerCrawler
{
    public IEnumerable<Issuer> Crawl(string directoryPath)
    {
        string issuerFilePath = Path.Combine(directoryPath, "m-issuer.json");
        bool fileExists = File.Exists(issuerFilePath);

        if (fileExists)
        {
            yield return ReadIssuer(issuerFilePath);
        }
        else
        {
            string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string subDirectoryPath in subDirectoryPaths)
            {
                IEnumerable<Issuer> issuers = Crawl(subDirectoryPath);

                foreach (Issuer issuer in issuers)
                    yield return issuer;
            }
        }
    }

    private static Issuer ReadIssuer(string filePath)
    {
        JsonFile<JIssuer> issuerFile = new(filePath);
        issuerFile.Open();

        Issuer issuer = issuerFile.Data.ToDomainEntity();
        issuer.Id = filePath;

        string rootDirectoryPath = Path.GetDirectoryName(filePath);

        IEnumerable<Emission> emissions = ReadEmissions(rootDirectoryPath);
        issuer.Emissions.AddRange(emissions);

        return issuer;
    }

    private static IEnumerable<Emission> ReadEmissions(string issuerDirectoryPath)
    {
        string[] subDirectories = Directory.GetDirectories(issuerDirectoryPath);

        foreach (string subDirectory in subDirectories)
        {
            EmissionCrawler emissionCrawler = new();
            IEnumerable<Emission> emissions = emissionCrawler.Crawl(subDirectory);

            foreach (Emission emission in emissions)
                yield return emission;
        }
    }
}