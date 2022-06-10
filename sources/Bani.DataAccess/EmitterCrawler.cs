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

            IEnumerable<Item> items = ReadItems(rootDirectoryPath);
            emission.Items.AddRange(items);

            return emission;
        }

        private static IEnumerable<Item> ReadItems(string emissionDirectoryPath)
        {
            string[] subDirectories = Directory.GetDirectories(emissionDirectoryPath);

            foreach (string subDirectory in subDirectories)
            {
                IEnumerable<Item> items = SearchForItems(subDirectory);

                foreach (Item item in items)
                    yield return item;
            }
        }

        private static IEnumerable<Item> SearchForItems(string directoryPath)
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
                IEnumerable<Item> items = SearchForItems(subDirectoryPath);

                foreach (Item item in items)
                    yield return item;
            }
        }

        private static IEnumerable<Coin> ReadCoins(string directoryPath)
        {
            ItemCrawler<JCoin> coinCrawler = new()
            {
                ItemFileName = "m-coin.json"
            };

            coinCrawler.Analyze(directoryPath);

            return coinCrawler.Items
                .Select(x => x.ToDomainEntity());
        }

        private static IEnumerable<Banknote> ReadBanknotes(string directoryPath)
        {
            ItemCrawler<JBanknote> banknoteCrawler = new()
            {
                ItemFileName = "m-banknote.json"
            };

            banknoteCrawler.Analyze(directoryPath);

            return banknoteCrawler.Items
                .Select(x => x.ToDomainEntity());
        }
    }
}