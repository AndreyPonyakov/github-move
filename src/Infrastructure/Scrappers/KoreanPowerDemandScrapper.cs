using System;
using System.IO;
using System.Net;
using System.Text;
using Domain.Entites;
using Infrastructure.Interfaces;

namespace Infrastructure.Scrappers
{
    public class KoreanPowerDemandScrapper : IScrapper
    {
        private string _mainlandURL;
        private readonly ITimeStampParser _timeStampParser;
        private readonly ISiteReader _siteReader;
        private readonly ICurrentLoadParser _currentLoadParser;

        public string Name { get; private set; }

        public KoreanPowerDemandScrapper(string mainlandURL, string name, ITimeStampParser timeStampParser, 
            ISiteReader siteReader, ICurrentLoadParser currentLoadParser)
        {
            _siteReader = siteReader;
            _timeStampParser = timeStampParser;
            _mainlandURL = mainlandURL;
            _currentLoadParser = currentLoadParser;
            Name = name;
        }

        public Observation GetData()
        {
            var content = _siteReader.GetHtmlContent();
            DateTimeOffset timeStamp = _timeStampParser.Parse(content);
            decimal value = _currentLoadParser.Parse(content);

            return new Observation(timeStamp, value);
        }
    }
}