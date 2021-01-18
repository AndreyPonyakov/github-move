using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.UnitTests
{
    public class SiteReaderTests
    {
        [Fact]
        public void GetSiteContent()
        {
            // Arrange
            var reader = new SiteReader("http://www.kpx.or.kr/www/contents.do?key=217");

            // Act
            var content = reader.GetHtmlContent();

            // Assert
            Assert.NotEmpty(content);
        }

        [Fact]
        public async Task GetSiteContentAsync()
        {
            // Arrange
            var reader = new SiteReader("http://www.kpx.or.kr/www/contents.do?key=217");

            // Act
            var content = await reader.GetHtmlContentAsync();

            // Assert
            Assert.NotEmpty(content);
        }

        [Fact]
        public void GetWrongSite()
        {
            // Arrange
            var reader = new SiteReader("http://test.test.test");

            // Act
            // Assert
            Assert.Throws<WebException>(() => reader.GetHtmlContent());
        }

        [Fact]
        public async Task GetWrongSiteAsync()
        {
            // Arrange
            var reader = new SiteReader("http://test.test.test");

            // Act
            // Assert
            await Assert.ThrowsAsync<WebException>(async () => await reader.GetHtmlContentAsync());
        }

    }
}