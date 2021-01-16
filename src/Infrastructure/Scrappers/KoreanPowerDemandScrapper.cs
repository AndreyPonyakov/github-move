using System;
using Domain.Entites;
using Infrastructure.Interfaces;

namespace Infrastructure.Scrappers
{
    public class KoreanPowerDemandScrapper : IScrapper
    {
        private string _mainlandURL;
        public string Name { get; private set; }


        public KoreanPowerDemandScrapper(string mainlandURL, string name)
        {
            _mainlandURL = mainlandURL;
            Name = name;
        }

        public Observation GetData()
        {
            return new Observation(DateTimeOffset.UtcNow, 1M);
        }
    }
}