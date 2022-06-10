using System;
using System.Collections.Generic;
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.DataAccess
{
    public class DbContext
    {
        private readonly string connectionString;

        public List<Emitter> Emitters { get; } = new();

        public DbContext(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            LoadEmitters();
        }

        private void LoadEmitters()
        {
            Emitters.Clear();

            EmitterCrawler emitterCrawler = new();
            IEnumerable<Emitter> emitters = emitterCrawler.SearchForEmitters(connectionString);
            Emitters.AddRange(emitters);
        }
    }
}