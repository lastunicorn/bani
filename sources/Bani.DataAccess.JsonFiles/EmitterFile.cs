using System;
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class EmitterFile
    {
        private readonly string filePath;

        public EmitterFile(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public JEmitter Emitter { get; set; }

        public void Open()
        {
            string json = File.ReadAllText(filePath);
            Emitter = JsonConvert.DeserializeObject<JEmitter>(json);
        }
    }
}