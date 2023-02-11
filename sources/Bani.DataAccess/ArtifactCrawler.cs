using System.Collections.Generic;
using System.IO;
using System.Linq;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess;

internal class ArtifactCrawler
{
    public IEnumerable<Artifact> Crawl(string directoryPath)
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
            IEnumerable<Artifact> items = Crawl(subDirectoryPath);

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