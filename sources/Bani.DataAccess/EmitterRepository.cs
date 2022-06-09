using System.Collections.Generic;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    public class EmitterRepository : IEmitterRepository
    {
        private readonly DbContext dbContext;

        public EmitterRepository()
        {
            dbContext = new DbContext();
        }

        public IEnumerable<Emitter> GetAll()
        {
            return dbContext.Emitters;
        }
    }
}