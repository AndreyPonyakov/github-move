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
    }
}