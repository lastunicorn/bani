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
            DbContext dbContext = new("/nfs/YubabaData/Alez/projects/Money/database");
            //DbContext dbContext = new(@"\\192.168.1.12\Data\Alez\projects\Money\database\");
            //DbContext dbContext = new(@"c:\Temp\database");

            IEmitterRepository emitterRepository = new EmitterRepository(dbContext);

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
                            Console.WriteLine($"    - {item.DisplayName} Count: {item.InstanceCount}");
                        else
                            Console.WriteLine($"    - {item.DisplayName} ({item.Year}) Count: {item.InstanceCount}");
                    }
                }
            }
        }
    }
}