using System.Collections.Generic;

namespace DustInTheWind.Bani.Domain
{
    public class Emission
    {
        public string Location { get; set; }
        
        public string Name { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public List<Item> Items { get; } = new();
    }
}