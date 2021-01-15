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

        public decimal GetData()
        {
            return 1;
        }
    }
}