using System;
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class CoinFile
    {
        public string FilePath { get; }

        public JCoin Coin { get; set; }

        public CoinFile(string filePath)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Open()
        {
            string json = File.ReadAllText(FilePath);
            Coin = JsonConvert.DeserializeObject<JCoin>(json);
        }
    }
}