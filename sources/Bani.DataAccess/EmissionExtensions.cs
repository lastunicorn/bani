using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    internal static class EmissionExtensions
    {
        public static Emission ToDomainEntity(this JEmission emission)
        {
            return new Emission
            {
                Name = emission.Name,
                StartYear = emission.StartYear,
                EndYear = emission.EndYear
            };
        }
    }
}