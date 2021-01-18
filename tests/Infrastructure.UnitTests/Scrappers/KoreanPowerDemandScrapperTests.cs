
using System;
using System.IO;
using Infrastructure.Interfaces;
using Infrastructure.Scrappers;
using Moq;
using Xunit;

namespace Infrastructure.UnitTests
{
    public class KoreanPowerDemandScrapperTests
    {
        [Fact]
        public void GetDataTest()
        {
            // Arrange
            var time = DateTimeOffset.Now;
            var value = 123M;
            var timeStampParser = new Mock<ITimeStampParser>();
            timeStampParser.Setup(x => x.Parse(It.IsAny<string>())).Returns(time);
            var siteReader = new Mock<ISiteReader>();
            var currentLoadParser = new Mock<ICurrentLoadParser>();
            currentLoadParser.Setup(x => x.Parse(It.IsAny<string>())).Returns(value);
            var scrapper =  new KoreanPowerDemandScrapper(
                "http://www.kpx.or.kr/www/contents.do?key=217", 
                "test", 
                timeStampParser.Object,
                siteReader.Object,
                currentLoadParser.Object);

            // Act
            var observation = scrapper.GetData();

            // Assert
            Assert.Equal(observation.CurrentLoad, value);
            Assert.Equal(observation.TimeStamp, time);
        }

    }
}