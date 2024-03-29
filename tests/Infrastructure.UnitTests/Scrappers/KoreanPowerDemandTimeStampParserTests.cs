using System;
using System.IO;
using Infrastructure.Exceptions;
using Infrastructure.Scrappers;
using Xunit;

namespace Infrastructure.UnitTests
{
    public class KoreanPowerDemandTimeStampParserTests
    {
        [Fact]
        public void GetTimeStampTest()
        {
            // Arrange
            var content = File.ReadAllText("./data/test1.txt");
            var parser = new KoreanPowerDemandTimeStampParser();

            // Act
            var timeStamp = parser.Parse(content);

            // Assert
            Assert.Equal(timeStamp, new DateTimeOffset(2021, 1, 17, 20, 50, 0, 0, TimeSpan.FromHours(9)));
        }

        [Fact]
        public void GetTimeStampFromEmptyContentTest()
        {
            // Arrange
            var content = "";
            var parser = new KoreanPowerDemandTimeStampParser();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => parser.Parse(content));
        }

        [Fact]
        public void GetTimeStampFromWrongContentTest()
        {
            // Arrange
            var content = File.ReadAllText("./data/WrongContent.txt"); ;
            var parser = new KoreanPowerDemandTimeStampParser();

            // Act
            // Assert
            Assert.Throws<ScrapperException>(() => parser.Parse(content));
        }
    }
}