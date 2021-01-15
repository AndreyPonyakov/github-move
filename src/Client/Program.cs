using System;
using Application.Observers;
using Infrastructure.Scrappers;
using Infrastructure.Storages;
using NLog;

namespace Client
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string MainlandTimeSeriesName = "koreanpowerdemand(Mainland)";
        private const string MainlandURL = "http://www.kpx.or.kr/www/contents.do?key=217";
        private const string JejuURL = "http://www.kpx.or.kr/www/contents.do?key=357";

        static void Main(string[] args)
        {
            var mainlandScrapper = new KoreanPowerDemandScrapper(MainlandURL, "Mainland");
            var mainlandStorage = new AngaraDataCatalogStorage(MainlandTimeSeriesName);
            var mainlandObserver = new Observer(mainlandScrapper, mainlandStorage, 5);           
            try
            {
                mainlandObserver.Run();

                Logger.Info("Service started");

                Console.WriteLine("press any key to stop application...");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                mainlandObserver.Stop();
            }

            Logger.Info("Service stopped");
            LogManager.Shutdown();
        }
    }
}
