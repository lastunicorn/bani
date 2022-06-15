using System;

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public class JBanknote : JItem
    {
        public float? Width { get; set; }

        public float? Height { get; set; }

        public bool? Embossing { get; set; }

        public int? PrintYear { get; set; }

        public JBanknote()
        {
        }

        public JBanknote(JBanknote banknote)
            : base(banknote)
        {
            if (banknote != null)
            {
                Width = banknote.Width;
                Height = banknote.Height;
                Embossing = banknote.Embossing;
                PrintYear = banknote.PrintYear;
            }
        }

        public override void MergeInto(JItem targetItem)
        {
            if (targetItem == null) throw new ArgumentNullException(nameof(targetItem));

            if (targetItem is JBanknote targetBanknote)
            {
                MergeInto(targetBanknote);
            }
            else
            {
                throw new ArgumentException($"The {nameof(targetItem)} must be a {typeof(JBanknote)}.", nameof(targetItem));
            }
        }

        private void MergeInto(JBanknote targetBanknote)
        {
            base.MergeInto(targetBanknote);

            if (Width != null) targetBanknote.Width = Width;
            if (Height != null) targetBanknote.Height = Height;
            if (Embossing != null) targetBanknote.Embossing = Embossing;
            if (PrintYear != null) targetBanknote.PrintYear = PrintYear;
        }
    }
}