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