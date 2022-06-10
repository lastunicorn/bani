namespace DustInTheWind.Bani.Domain
{
    public class Banknote : Item
    {
        public float? Width { get; set; }
        
        public float? Height { get; set; }
        
        public bool? Embossing { get; set; }
    }
}