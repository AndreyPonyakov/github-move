using System;
using Application.Observers;
using Infrastructure.Scrappers;
using Infrastructure.Storages;

namespace Client
{
    class Program
    {
        private const string MainlandTimeSeriesName = "koreanpowerdemand(Mainland)";
        private const string MainlandURL = "http://www.kpx.or.kr/www/contents.do?key=217";
        private const string JejuURL = "http://www.kpx.or.kr/www/contents.do?key=357";

        static void Main(string[] args)
        {
            var mainlandScrapper = new KoreanPowerDemandScrapper(MainlandURL);
            var mainlandStorage = new AngaraDataCatalogStorage(MainlandTimeSeriesName);
            var mainlandObserver = new Observer(mainlandScrapper, mainlandStorage);           
            try
            {


                mainlandObserver.Run();

                Console.WriteLine("press any key to stop application...");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                // ToDo add logging
            }
            finally
            {
                mainlandObserver.Stop();
            }
        }
    }
}
