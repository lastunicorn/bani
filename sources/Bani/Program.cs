using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.Bani.DataAccess;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            IEmitterRepository emitterRepository = new EmitterRepository();

            IEnumerable<Emitter> emitters = emitterRepository.GetAll();

            if (args.Length > 0)
            {
                string emitterName = args[0];
                
                emitters = emitters
                    .Where(x => x.Name?.Contains(emitterName, StringComparison.InvariantCultureIgnoreCase) ?? false);
            }

            foreach (Emitter emitter in emitters)
            {
                Console.WriteLine($"{emitter.Name}");

                foreach (Emission emission in emitter.Emissions)
                {
                    Console.WriteLine($"  - {emission.Name} [{emission.StartYear}-{emission.EndYear}]");

                    foreach (Item item in emission.Items)
                    {
                        if (item.Year == null)
                            Console.WriteLine($"    - {item.DisplayName}");
                        else
                            Console.WriteLine($"    - {item.DisplayName} ({item.Year}) Count: {item.InstanceCount}");
                    }
                }
            }
        }
    }
}