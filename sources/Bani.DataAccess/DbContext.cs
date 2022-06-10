using System;
using System.Collections.Generic;
using System.IO;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    public class DbContext
    {
        private readonly string connectionString;

        public List<Emitter> Emitters { get; } = new();

        public DbContext(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            LoadEmitters();
        }

        private void LoadEmitters()
        {
            Emitters.Clear();

            IEnumerable<Emitter> emitters = SearchForEmitters(connectionString);
            Emitters.AddRange(emitters);
        }

        private static IEnumerable<Emitter> SearchForEmitters(string directoryPath)
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
            string[] coinFilePaths = Directory.GetFiles(directoryPath, "m-coin.json");

            if (coinFilePaths.Length > 0)
            {
                IEnumerable<Coin> coins = ReadCoins(coinFilePaths[0]);

                foreach (Coin coin in coins)
                    yield return coin;

                yield break;
            }

            string[] banknoteFilePaths = Directory.GetFiles(directoryPath, "m-banknote.json");

            if (banknoteFilePaths.Length > 0)
            {
                yield return ReadBanknote(banknoteFilePaths[0]);
                yield break;
            }

            string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string subDirectoryPath in subDirectoryPaths)
            {
                IEnumerable<Item> items = SearchForItems(subDirectoryPath);

                foreach (Item item in items)
                    yield return item;
            }
        }

        private static IEnumerable<Coin> ReadCoins(string filePath)
        {
            CoinFile coinFile = new(filePath);
            coinFile.Open();

            IEnumerable<JCoin> subCoins = ReadSubCoins(coinFile);

            bool isAnySubCoin = false;
            foreach (JCoin jSubCoin in subCoins)
            {
                Coin subCoin = jSubCoin.ToDomainEntity();
                subCoin.Location = filePath;
                yield return subCoin;
                isAnySubCoin = true;
            }

            if (!isAnySubCoin)
            {
                Coin coin = coinFile.Coin.ToDomainEntity();
                coin.Location = filePath;

                yield return coin;
            }
        }

        private static IEnumerable<JCoin> ReadSubCoins(CoinFile coinFile)
        {
            string directoryPath = Path.GetDirectoryName(coinFile.FilePath);
            string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string subDirectoryPath in subDirectoryPaths)
            {
                IEnumerable<JCoin> coins = SearchForCoins(subDirectoryPath);

                foreach (JCoin subCoin in coins)
                {
                    JCoin mergedCoin = JCoin.Merge(coinFile.Coin, subCoin);
                    yield return mergedCoin;
                }
            }
        }

        private static IEnumerable<JCoin> SearchForCoins(string directoryPath)
        {
            string[] coinFilePaths = Directory.GetFiles(directoryPath, "m-coin.json");

            if (coinFilePaths.Length > 0)
            {
                CoinFile coinFile = new(coinFilePaths[0]);
                coinFile.Open();

                yield return coinFile.Coin;
            }
        }

        private static Banknote ReadBanknote(string filePath)
        {
            BanknoteFile coinFile = new(filePath);
            coinFile.Open();

            Banknote banknote = coinFile.Banknote.ToDomainEntity();
            banknote.Location = filePath;

            return banknote;
        }
    }
}