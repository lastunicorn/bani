using System;
using System.Collections.Generic;
using System.IO;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
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
}