using System;
using System.Linq;
using Domain.Entites;
using Gazprom.Angara.Client.Storage;
using Gazprom.Angara.Contract.Entities.TimeSeries;
using Gazprom.Angara.Contract.Messages;
using Gazprom.Core.Contract.Entities.Http;
using Infrastructure.Exceptions;
using Infrastructure.Storages;
using Moq;
using Xunit;

namespace Infrastructure.UnitTests
{
    public class AngaraDataCatalogStorageTests
    {
        [Fact]
        public void SaveObservationTest()
        {
            // Arrange
            var scrapeName = "test series";
            var curveId = "1234";
            var client = new Mock<IDataStorageClient>();
            client.Setup(c => c.StoreOne<TimeSeriesData>(It.IsAny<StoreDataRequest<TimeSeriesData>>()))
                .Returns(new DataResponse<StoreDataResponse>());
            var storage =  new AngaraDataCatalogStorage(scrapeName, curveId, client.Object);
            var observation = new Observation(DateTime.Now, 1M);

            // Act
            storage.Save(observation);

            // Assert
            client.Verify(x => x.StoreOne<TimeSeriesData>(
                It.Is<StoreDataRequest<TimeSeriesData>>(tsd => 
                    tsd.DataPayload.Data.Points.First().Value == observation.CurrentLoad
                    && tsd.DataPayload.Data.Points.First().DatePoint == observation.TimeStamp
                    && tsd.DataId.Id == curveId
                    && tsd.CaptureSystem == scrapeName
                    && tsd.DataId.Class == "curve"
                    && tsd.DataId.Grade == "raw")));

        }

        [Fact]
        public void FailedSaveObservationTest()
        {
            // Arrange
            var client = new Mock<IDataStorageClient>();
            client.Setup(c => c.StoreOne<TimeSeriesData>(It.IsAny<StoreDataRequest<TimeSeriesData>>()))
                .Returns(new DataResponse<StoreDataResponse> { Faulted = true });
            var storage =  new AngaraDataCatalogStorage("test series", "1234", client.Object);
            var observation = new Observation(DateTime.Now, 1M);

            // Act
            Assert.Throws<StorageException>(() => storage.Save(observation));

            // Assert
        }

    }
}
