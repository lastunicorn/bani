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
using System.Linq;
using DustInTheWind.Bani.Adapters.DataAccess;
using DustInTheWind.Bani.Adapters.DataAccess.DataMapping;
using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess;

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