using System;
using HtmlAgilityPack;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;

namespace Infrastructure.Scrappers
{

    public class KoreanPowerDemandTimeStampParser : ITimeStampParser
    {
        public DateTimeOffset Parse(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");
                
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);

            string timeNode;

            try
            {
                timeNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='powerinfo_title']/p").InnerText;
            }
            catch (Exception ex)
            {
                throw new ScrapperException("Cann't scrape time from the site", ex);
            }

            try
            {            
                var year = int.Parse(timeNode.Substring(0, 4));
                var month = int.Parse(timeNode.Substring(6, 2));
                var day = int.Parse(timeNode.Substring(10, 2));
                var hour = int.Parse(timeNode.Substring(16, 2));
                var minute = int.Parse(timeNode.Substring(19, 2));

                return new DateTimeOffset(year, month, day, hour, minute, 0, 0, TimeSpan.FromHours(9));
            }
            catch (Exception ex)
            {
                throw new ScrapperException("Cann't parse time from the site", ex);
            }
        }
    }
}