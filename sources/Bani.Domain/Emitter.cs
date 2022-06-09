using System.Collections.Generic;

namespace DustInTheWind.Bani.Domain
{
    public class Emitter
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public List<Emission> Emissions { get; } = new();
    }
}