using System;
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class EmissionFile
    {
        private readonly string filePath;

        public EmissionFile(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public JEmission Emission { get; set; }

        public void Open()
        {
            string json = File.ReadAllText(filePath);
            Emission = JsonConvert.DeserializeObject<JEmission>(json);
        }
    }
}