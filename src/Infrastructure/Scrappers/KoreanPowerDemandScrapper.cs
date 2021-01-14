using Infrastructure.Interfaces;

namespace Infrastructure.Scrappers
{
    public class KoreanPowerDemandScrapper : IScrapper
    {
        private string mainlandURL;

        public KoreanPowerDemandScrapper(string mainlandURL)
        {
            this.mainlandURL = mainlandURL;
        }
    }
}