using System;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class JCoin : JItem
    {
        public float? Diameter { get; set; }

        public string Edge { get; set; }

        public JCoin()
        {
        }

        public JCoin(JCoin coin)
            : base(coin)
        {
            if (coin != null)
            {
                Diameter = coin.Diameter;
                Edge = coin.Edge;
            }
        }

        public override void MergeInto(JItem targetItem)
        {
            if (targetItem == null) throw new ArgumentNullException(nameof(targetItem));

            if (targetItem is JCoin targetCoin)
            {
                MergeInto(targetCoin);
            }
            else
            {
                throw new ArgumentException($"The {nameof(targetItem)} must be a {typeof(JCoin)}.", nameof(targetItem));
            }
        }

        private void MergeInto(JCoin targetCoin)
        {
            base.MergeInto(targetCoin);

            if (Diameter != null) targetCoin.Diameter = Diameter;
            if (Edge != null) targetCoin.Edge = Edge;
        }
    }
}