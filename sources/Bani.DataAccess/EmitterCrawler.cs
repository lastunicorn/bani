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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    internal class EmitterCrawler
    {
        public IEnumerable<Emitter> SearchForEmitters(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath, "m-emitter.json");

            if (filePaths.Length > 0)
            {
                yield return ReadEmitter(filePaths[0]);
            }
            else
            {
                string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);

                foreach (string subDirectoryPath in subDirectoryPaths)
                {
                    IEnumerable<Emitter> emitters = SearchForEmitters(subDirectoryPath);

                    foreach (Emitter emitter in emitters)
                        yield return emitter;
                }
            }
        }

        private static Emitter ReadEmitter(string filePath)
        {
            EmitterFile emitterFile = new(filePath);
            emitterFile.Open();

            Emitter emitter = emitterFile.Emitter.ToDomainEntity();
            emitter.Location = filePath;

            string rootDirectoryPath = Path.GetDirectoryName(filePath);

            IEnumerable<Emission> emissions = ReadEmissions(rootDirectoryPath);
            emitter.Emissions.AddRange(emissions);

            return emitter;
        }

        private static IEnumerable<Emission> ReadEmissions(string emitterDirectoryPath)
        {
            string[] subDirectories = Directory.GetDirectories(emitterDirectoryPath);

            foreach (string subDirectory in subDirectories)
            {
                IEnumerable<Emission> emissions = SearchForEmissions(subDirectory);

                foreach (Emission emission in emissions)
                    yield return emission;
            }
        }

        private static IEnumerable<Emission> SearchForEmissions(string directoryPath)
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
                    IEnumerable<Emission> emissions = SearchForEmissions(subDirectoryPath);

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
            emission.Location = filePath;

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
                IEnumerable<Artifact> artifacts = SearchForArtifacts(subDirectory);

                foreach (Artifact item in artifacts)
                    yield return item;
            }
        }

        private static IEnumerable<Artifact> SearchForArtifacts(string directoryPath)
        {
            bool itemsFound = false;

            IEnumerable<Coin> coins = ReadCoins(directoryPath);

            foreach (Coin coin in coins)
            {
                itemsFound = true;
                yield return coin;
            }

            IEnumerable<Banknote> banknotes = ReadBanknotes(directoryPath);

            foreach (Banknote banknote in banknotes)
            {
                itemsFound = true;
                yield return banknote;
            }

            if (itemsFound)
                yield break;

            string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string subDirectoryPath in subDirectoryPaths)
            {
                IEnumerable<Artifact> items = SearchForArtifacts(subDirectoryPath);

                foreach (Artifact item in items)
                    yield return item;
            }
        }

        private static IEnumerable<Coin> ReadCoins(string directoryPath)
        {
            CoinDirectory coinDirectory = new(directoryPath);
            coinDirectory.Read();

            return coinDirectory.Artifacts
                .Select(x => x.ToDomainEntity());
        }

        private static IEnumerable<Banknote> ReadBanknotes(string directoryPath)
        {
            BanknoteDirectory banknoteDirectory = new(directoryPath);
            banknoteDirectory.Read();

            return banknoteDirectory.Artifacts
                .Select(x => x.ToDomainEntity());
        }
    }
}