using System;
using Application.Observers;
using Gazprom.Angara.Client.Contract;
using Gazprom.Angara.Client.Storage;
using Infrastructure.Scrappers;
using Infrastructure.Storages;
using NLog;

namespace Client
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string MainlandScrapeName = "koreanpowerdemand(Mainland)";
        private const string JejuScrapeName = "koreanpowerdemand(Jeju)";
        private const string MainlandURL = "http://www.kpx.or.kr/www/contents.do?key=217";
        private const string MainlandCurveId = "P_TW_Demand_Current_Load_Mainland";
        private const string JejuURL = "http://www.kpx.or.kr/www/contents.do?key=357";
        private const string JejuCurveId = "P_TW_Demand_Current_Load_Jeju";
        private const string BaseUrl = "http://localhost:5555/angara/storage";


        static void Main(string[] args)
        {
            Observer mainlandObserver = CreateObserver(MainlandScrapeName, MainlandCurveId);
            try
            {
                mainlandObserver.Run();

                Logger.Info("Service started");

                Console.WriteLine("press any key to stop application...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                mainlandObserver.Stop();
                mainlandObserver.Dispose();
            }

            Logger.Info("Service stopped");
            LogManager.Shutdown();
        }

        private static Observer CreateObserver(string scrapperName, string curveId)
        {
            var mainlandScrapper = new KoreanPowerDemandScrapper(MainlandURL, scrapperName);
            var config = new ServiceClientConfig
            {
                ServiceBaseUrl = BaseUrl,
                Version = "1"
            };
            var client = new DataStorageClient(config);
            var mainlandStorage = new AngaraDataCatalogStorage(scrapperName, curveId, client);
            return new Observer(mainlandScrapper, mainlandStorage, 5);
        }
    }
}
