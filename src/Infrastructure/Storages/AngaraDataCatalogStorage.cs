using System;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(seriesName))
                throw new ArgumentNullException("seriesName");

            if (string.IsNullOrEmpty(curveId))
                throw new ArgumentNullException("curveId");

            if (client == null)
                throw new ArgumentNullException("client");


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
            if (observation == null)
                throw new ArgumentNullException("observation");
            
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

        public Task SaveAsync(Observation observation)
        {
            if (observation == null)
                throw new ArgumentNullException("observation");

            var tcs = new TaskCompletionSource<object>();

            var timeseries = MakeTimeseriesData(observation);

            var request = CreateRequest();
            request.SetDataPayload(timeseries);

            Logger.Debug($"Sending StoreDataRequest for {request.ActivityId}/{request.DataId?.GlobalId()}...");
            var response = _client.StoreOne(request);

            if (response.Faulted)
            {
                tcs.SetException(new StorageException(response.Message));
            }
            else
            {
                tcs.SetResult(null);
            }            

            return tcs.Task;
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
                CaptureSystem = _seriesName,
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