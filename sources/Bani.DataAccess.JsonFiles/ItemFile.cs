using System;
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class ItemFile<T>
        where T : JItem
    {
        public string FilePath { get; }

        public T Item { get; set; }

        public ItemFile(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Open()
        {
            string json = File.ReadAllText(FilePath);
            Item = JsonConvert.DeserializeObject<T>(json);

            if (Item != null)
                Item.Location = FilePath;
        }
    }
}