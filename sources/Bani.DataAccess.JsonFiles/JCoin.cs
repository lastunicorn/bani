namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class JCoin : JItem
    {
        public float? Diameter { get; set; }

        public string Edge { get; set; }

        public static JCoin Merge(JCoin coin1, JCoin coin2)
        {
            return new JCoin
            {
                DisplayName = coin2.DisplayName ?? coin1.DisplayName,
                Value = coin2.Value ?? coin1.Value,
                Unit = coin2.Unit ?? coin1.Unit,
                Substance = coin2.Substance ?? coin1.Substance,
                Color = coin2.Color ?? coin1.Color,
                Year = coin2.Year ?? coin1.Year,
                Diameter = coin2.Diameter ?? coin1.Diameter,
                Obverse = coin2.Obverse ?? coin1.Obverse,
                Reverse = coin2.Reverse ?? coin1.Reverse,
                Edge = coin2.Edge ?? coin1.Edge,
                InstanceCount = coin2.InstanceCount ?? coin1.InstanceCount
            };
        }
    }
}