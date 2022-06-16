// Bani
// Copyright (C) 2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class JCoin : JItem
    {
        public float? Diameter { get; set; }

        public string Edge { get; set; }

        public int? MintYear { get; set; }

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
                MintYear = coin.MintYear;
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
            if (MintYear != null) targetCoin.MintYear = MintYear;
        }
    }
}