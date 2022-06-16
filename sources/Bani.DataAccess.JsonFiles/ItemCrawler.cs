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

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class ItemCrawler<T>
        where T : JItem
    {
        private readonly Stack<T> stack = new();

        public string ItemFileName { get; set; } = "m-coin.json";

        public List<T> Items { get; private set; }

        public void Analyze(string directoryPath)
        {
            stack.Clear();
            Items = new List<T>();

            ProcessDirectory(directoryPath);
        }

        private bool ProcessDirectory(string directoryPath)
        {
            string itemFilePath = Path.Combine(directoryPath, ItemFileName);
            bool itemFileExists = File.Exists(itemFilePath);

            if (itemFileExists)
                ReadItemAndPushToStack(itemFilePath);

            IEnumerable<string> subDirectoryPaths = Directory.EnumerateDirectories(directoryPath);

            bool subItemWasFound = false;

            foreach (string subDirectoryPath in subDirectoryPaths)
                subItemWasFound |= ProcessDirectory(subDirectoryPath);

            if (itemFileExists)
            {
                T currentItem = stack.Pop();

                if (!subItemWasFound)
                    Items.Add(currentItem);
            }

            return subItemWasFound || itemFileExists;
        }

        private void ReadItemAndPushToStack(string itemFilePath)
        {
            ItemFile<T> itemFile = new(itemFilePath);
            itemFile.Open();

            T currentItem = itemFile.Item;

            if (stack.Count > 0)
            {
                T stackItem = stack.Peek();
                T newItem = stackItem.Clone() as T;
                currentItem.MergeInto(newItem);
                stack.Push(newItem);
            }
            else
            {
                stack.Push(currentItem);
            }
        }
    }
}