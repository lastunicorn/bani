using System.Collections.Generic;

namespace DustInTheWind.Bani.Domain
{
    public interface IEmitterRepository
    {
        IEnumerable<Emitter> GetAll();
    }
}