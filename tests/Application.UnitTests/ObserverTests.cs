using System;
using System.Threading;
using Application.Observers;
using Domain.Entites;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace Application.UnitTests
{
    public class ObserverTests
    {
        [Fact]
        public void RunTwiceByScheduleAndStoreData()
        {
            // Arrange
            const double ONE_SECOND = 1.0 / 60;
            var scrapper = new Mock<IScrapper>();
            var storage = new Mock<IStorage>();
            var observer = new Observer(scrapper.Object, storage.Object, ONE_SECOND);

            // Act
            observer.Run();
            Thread.Sleep(1200);
            observer.Stop();

            // Assert
            scrapper.Verify(x => x.GetData(), Times.Exactly(2));
            storage.Verify(x => x.Save(It.IsAny<Observation>()), Times.Exactly(2));
        }
    }
}
