using DustInTheWind.Bani.DataAccess.JsonFiles;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    internal static class EmitterExtensions
    {
        public static Emitter ToDomainEntity(this JEmitter emitter)
        {
            return new Emitter
            {
                Name = emitter.Name
            };
        }
    }
}