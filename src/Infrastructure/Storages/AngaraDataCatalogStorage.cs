using System;
using Domain.Entites;
using Gazprom.Angara.Client.Contract;
using Gazprom.Angara.Client.Storage;
using Gazprom.Angara.Contract.Entities;
using Gazprom.Angara.Contract.Entities.TimeSeries;
using Gazprom.Angara.Contract.Messages;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using NLog;

namespace Infrastructure.Storages
{
    public sealed class AngaraDataCatalogStorage : IStorage
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string _seriesName;
        private string _curveId;
        private IDataStorageClient _client;

        public AngaraDataCatalogStorage(string seriesName, string curveId, IDataStorageClient client)
        {
            _seriesName = seriesName;
            _client = client;
            _curveId = curveId;
        }

        ~AngaraDataCatalogStorage()
        {
            Dispose(false);
        }

        public void Save(Observation observation)
        {
            var timeseries = MakeTimeseriesData(observation);

            var request = CreateRequest();
            request.SetDataPayload(timeseries);

            Logger.Debug($"Sending StoreDataRequest for {request.ActivityId}/{request.DataId?.GlobalId()}...");
            var response = _client.StoreOne(request);

            if (response.Faulted)
            {
                throw new StorageException(response.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private TimeSeriesData MakeTimeseriesData(Observation observation)
        {
            var timeseries = new TimeSeriesData
            {
                Points = new []
                {
                    new TimeSeriesObservation
                    {
                        DatePoint = observation.TimeStamp,
                        Value = observation.CurrentLoad
                    }
                }
            };

            return timeseries;
        }

        private StoreDataRequest<TimeSeriesData> CreateRequest()
        {
            var effectiveDate = new DateTimeOffset(DateTime.Today);
            var dataId = new DataIdentifier
            {	
                Origin = "TestTask",
                Class = "curve",
                Grade = "raw",
                Id = _curveId,
            };

            var request = new StoreDataRequest<TimeSeriesData>
            {
                CaptureSystem = "LinqPadDemo-StoreOne-TSD",
                CaptureTimeStampUtc = DateTime.UtcNow,		
                EffectiveOn = effectiveDate,
                DataId = dataId
            };

            return request;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            _client.Dispose();
            _client = null;
        }
    }
}