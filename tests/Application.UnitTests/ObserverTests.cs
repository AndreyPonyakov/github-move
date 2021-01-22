using System;
using System.Threading;
using System.Threading.Tasks;
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
            scrapper.Setup(s => s.GetDataAsync()).Returns(Task.FromResult(new Observation(DateTimeOffset.UtcNow, 1M)));
            var storage = new Mock<IStorage>();
            var observer = new Observer(scrapper.Object, storage.Object, ONE_SECOND);

            // Act
            observer.Run();
            Thread.Sleep(1200);
            observer.Stop();

            // Assert
            scrapper.Verify(x => x.GetDataAsync(), Times.Exactly(2));
            storage.Verify(x => x.Save(It.IsAny<Observation>()), Times.Exactly(2));
        }

        [Fact]
        public void RunTwiceByScheduleWithResavingObservations()
        {
            // Arrange
            const double ONE_SECOND = 1.0 / 60;
            var scrapper = new Mock<IScrapper>();
            scrapper.Setup(s => s.GetDataAsync()).Returns(Task.FromResult(new Observation(DateTimeOffset.UtcNow, 1M)));
            var storage = new Mock<IStorage>();
            storage.Setup(x => x.Save(It.IsAny<Observation>())).Callback(() => throw new Exception("test"));
            var observer = new Observer(scrapper.Object, storage.Object, ONE_SECOND);

            // Act
            observer.Run();
            Thread.Sleep(1200);
            observer.Stop();

            // Assert
            scrapper.Verify(x => x.GetDataAsync(), Times.Exactly(2));
            storage.Verify(x => x.Save(It.IsAny<Observation>()), Times.Exactly(3));
        }
    }
}
