﻿using System;
using Application.Observers;
using Gazprom.Angara.Client.Contract;
using Infrastructure.FileStorage;
using Infrastructure.Scrappers;
using NLog;

namespace Client
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string BaseUrl = "http://localhost:5555/angara/storage";


        static void Main(string[] args)
        {
            var mainlandObserver = CreateObserver(
                "koreanpowerdemand(Mainland)", 
                "P_TW_Demand_Current_Load_Mainland",
                "http://www.kpx.or.kr/www/contents.do?key=217");

            var jejuObserver = CreateObserver(
                "koreanpowerdemand(Jeju)", 
                "P_TW_Demand_Current_Load_Jeju",
                "http://www.kpx.or.kr/www/contents.do?key=357");

            try
            {
                mainlandObserver.Run();
                jejuObserver.Run();

                Logger.Info("Service started");

                Console.WriteLine("Press any key to stop the application...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unhandled exception");
            }
            finally
            {
                mainlandObserver.Stop();
                mainlandObserver.Dispose();

                jejuObserver.Stop();
                jejuObserver.Dispose();
            }

            Logger.Info("Service stopped");
            LogManager.Shutdown();
        }

        private static Observer CreateObserver(string scrapperName, string curveId, string url)
        {
            var timeStampParser = new KoreanPowerDemandTimeStampParser();
            var siteReader = new SiteReader(url);
            var currentLoadParser = new KoreanPowerDemandCurrentLoadParser();
            var scrapper = new KoreanPowerDemandScrapper(url, scrapperName, timeStampParser, siteReader, currentLoadParser);
            var config = new ServiceClientConfig
            {
                ServiceBaseUrl = BaseUrl,
                Version = "1"
            };
            // var client = new DataStorageClient(config);
            // var storage = new AngaraDataCatalogStorage(scrapperName, curveId, client);
            var storage = new FileStorage(scrapperName);
            return new Observer(scrapper, storage, 5);
        }
    }
}
