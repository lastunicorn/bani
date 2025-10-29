// Bani
// Copyright (C) 2022-2025 Dust in the Wind
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

namespace DustInTheWind.Bani.DataAccess.JsonFiles;

public class JBanknote : JArtifact
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

    public override void MergeInto(JArtifact targetArtifact)
    {
        if (targetArtifact == null) throw new ArgumentNullException(nameof(targetArtifact));

        if (targetArtifact is JBanknote targetBanknote)
        {
            MergeInto(targetBanknote);
        }
        else
        {
            throw new ArgumentException($"The {nameof(targetArtifact)} must be a {typeof(JBanknote)}.", nameof(targetArtifact));
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