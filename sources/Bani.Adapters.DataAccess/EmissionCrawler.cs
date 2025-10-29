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

internal class EmissionCrawler
{
    public IEnumerable<Emission> Crawl(string directoryPath)
    {
        string[] filePaths = Directory.GetFiles(directoryPath, "m-emission.json");

        if (filePaths.Length > 0)
        {
            yield return ReadEmission(filePaths[0]);
        }
        else
        {
            string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string subDirectoryPath in subDirectoryPaths)
            {
                IEnumerable<Emission> emissions = Crawl(subDirectoryPath);

                foreach (Emission emission in emissions)
                    yield return emission;
            }
        }
    }

    private static Emission ReadEmission(string filePath)
    {
        EmissionFile emissionFile = new(filePath);
        emissionFile.Open();

        Emission emission = emissionFile.Emission.ToDomainEntity();

        string rootDirectoryPath = Path.GetDirectoryName(filePath);

        IEnumerable<Artifact> artifacts = ReadArtifacts(rootDirectoryPath);
        emission.Artifacts.AddRange(artifacts);

        return emission;
    }

    private static IEnumerable<Artifact> ReadArtifacts(string emissionDirectoryPath)
    {
        string[] subDirectories = Directory.GetDirectories(emissionDirectoryPath);

        foreach (string subDirectory in subDirectories)
        {
            ArtifactCrawler artifactCrawler = new();
            IEnumerable<Artifact> artifacts = artifactCrawler.Crawl(subDirectory);

            foreach (Artifact item in artifacts)
                yield return item;
        }
    }
}