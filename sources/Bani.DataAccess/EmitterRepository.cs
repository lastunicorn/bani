using System;
using System.Collections.Generic;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    public class EmitterRepository : IEmitterRepository
    {
        private readonly DbContext dbContext;

        public EmitterRepository(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IEnumerable<Emitter> GetAll()
        {
            return dbContext.Emitters;
        }
    }
}