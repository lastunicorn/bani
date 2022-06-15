using System;

namespace DustInTheWind.Bani.Domain
{
    public class Item
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public float? Value { get; set; }

        public string Unit { get; set; }

        public string Substance { get; set; }

        public string Color { get; set; }
        
        public DateTime? IssueDate { get; set; }
        
        public int? Year { get; set; }

        public Picture Obverse { get; set; }
        
        public Picture Reverse { get; set; }
        
        public int InstanceCount { get; set; }
    }
}