using System.Drawing;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public abstract class JItem
    {
        public string DisplayName { get; set; }

        public float? Value { get; set; }

        public string Unit { get; set; }

        public string Substance { get; set; }

        public string Color { get; set; }
        
        public int? Year { get; set; }

        public JPicture Obverse { get; set; }
        
        public JPicture Reverse { get; set; }
        
        public int? InstanceCount { get; set; }
    }
}