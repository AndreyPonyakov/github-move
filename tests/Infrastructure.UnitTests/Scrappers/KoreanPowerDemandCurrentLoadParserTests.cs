using System;
using System.IO;
using Infrastructure.Exceptions;
using Xunit;

namespace Infrastructure.UnitTests
{
    public class KoreanPowerDemandCurrentLoadParserTests
    {
        [Fact]
        public void GetCurrentLoadWithMillionTest()
        {
            // Arrange
            var content = File.ReadAllText("./data/test1.txt");
            var parser =  new KoreanPowerDemandCurrentLoadParser();

            // Act
            var currentLoad = parser.Parse(content);

            // Assert
            Assert.Equal(6581200000M, currentLoad);
        }

        [Fact]
        public void GetCurrentLoadWithThousandTest()
        {
            // Arrange
            var content = File.ReadAllText("./data/test2.txt");
            var parser =  new KoreanPowerDemandCurrentLoadParser();

            // Act
            var currentLoad = parser.Parse(content);

            // Assert
            Assert.Equal(848000M, currentLoad);
        }

        [Fact]
        public void GetCurrentLoadFromEmptyContentTest()
        {
            // Arrange
            var content = "";
            var parser =  new KoreanPowerDemandCurrentLoadParser();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => parser.Parse(content));
        }

        [Fact]
        public void GetCurrentLoadFromWrongContentTest()
        {
            // Arrange
            var content = File.ReadAllText("./data/WrongContent.txt");;
            var parser =  new KoreanPowerDemandCurrentLoadParser();

            // Act
            // Assert
            Assert.Throws<ScrapperException>(() => parser.Parse(content));
        }   
    }
}