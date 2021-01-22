using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;
using Xunit;

namespace Infrastructure.FileStorage.UnitTests
{
    public class FileStorageTests
    {
        [Fact]
        public async Task SaveObservationTest()
        {
            // Arrange
            var seriasName = "test";
            var fileName = seriasName + ".txt";
            File.Delete(fileName);
            var storage =  new FileStorage(seriasName);
            var observation = new Observation(DateTime.Now, 1M);

            // Act
            await storage.SaveAsync(observation);

            // Assert
            Assert.True(File.Exists(fileName));
        }
    }
}
