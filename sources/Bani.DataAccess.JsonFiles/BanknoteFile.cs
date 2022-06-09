using System;
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class BanknoteFile
    {
        private readonly string filePath;

        public JBanknote Banknote { get; set; }

        public BanknoteFile(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Open()
        {
            string json = File.ReadAllText(filePath);
            Banknote = JsonConvert.DeserializeObject<JBanknote>(json);
        }
    }
}