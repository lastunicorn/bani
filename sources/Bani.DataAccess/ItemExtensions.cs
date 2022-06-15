using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    internal static class ItemExtensions
    {
        public static Coin ToDomainEntity(this JCoin coin)
        {
            return new Coin
            {
                Id = coin.Location,
                DisplayName = coin.DisplayName,
                Value = coin.Value,
                Unit = coin.Unit,
                Substance = coin.Substance,
                Color = coin.Color,
                Diameter = coin.Diameter,
                IssueDate = coin.IssueDate,
                Year = coin.MintYear,
                Obverse = coin.Obverse.ToDomainEntity(),
                Reverse = coin.Reverse.ToDomainEntity(),
                Edge = coin.Edge,
                InstanceCount = coin.InstanceCount ?? 0
            };
        }

        public static Banknote ToDomainEntity(this JBanknote banknote)
        {
            return new Banknote
            {
                Id = banknote.Location,
                DisplayName = banknote.DisplayName,
                Value = banknote.Value,
                Unit = banknote.Unit,
                Substance = banknote.Substance,
                Color = banknote.Color,
                Width = banknote.Width,
                Height = banknote.Height,
                Embossing = banknote.Embossing,
                IssueDate = banknote.IssueDate,
                Year = banknote.PrintYear,
                Obverse = banknote.Obverse.ToDomainEntity(),
                Reverse = banknote.Reverse.ToDomainEntity(),
                InstanceCount = banknote.InstanceCount ?? 0
            };
        }

        private static Picture ToDomainEntity(this JPicture picture)
        {
            if (picture == null)
                return null;

            return new Picture
            {
                FilePath = picture.FilePath,
                Bytes = picture.Bytes,
                Description = picture.Description
            };
        }
    }
}