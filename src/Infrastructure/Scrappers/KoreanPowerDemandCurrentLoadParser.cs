using System;
using System.Globalization;
using HtmlAgilityPack;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;

public class KoreanPowerDemandCurrentLoadParser : ICurrentLoadParser
{
    public decimal Parse(string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new ArgumentNullException("content");

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(content);

        string currentLoadNode;

        try
        {
            currentLoadNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='powerinfo']//tr[2]/td").InnerText;
        }
        catch (Exception ex)
        {
            throw new ScrapperException("Cann't scrape current load data from the site", ex);
        }

        try
        {            
            var firstSpaceIndex = currentLoadNode.IndexOf(' ');
            var currentLoad = decimal.Parse(currentLoadNode.Substring(0, firstSpaceIndex));

            var mu = currentLoadNode.Substring(currentLoadNode.Length - 4);

            switch (mu)
            {
                case "만 kW":
                    currentLoad *= 1000000;
                break;
                case "천 kW":
                    currentLoad *= 1000;
                break;
                default:
                    throw new ScrapperException($"Unknown measurement unit - {mu}");
            }

            return currentLoad;
        }
        catch (ScrapperException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ScrapperException("Cann't parse current load data from the site", ex);
        }
    }
}