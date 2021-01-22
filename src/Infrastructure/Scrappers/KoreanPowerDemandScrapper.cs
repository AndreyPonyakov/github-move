using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(mainlandURL))
                throw new ArgumentNullException("mainlandURL");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (timeStampParser == null)
                throw new ArgumentNullException("timeStampParser");

            if (siteReader == null)
                throw new ArgumentNullException("siteReader");

            if (currentLoadParser == null)
                throw new ArgumentNullException("currentLoadParser");

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

        public async Task<Observation> GetDataAsync()
        {
            var content = await _siteReader.GetHtmlContentAsync();
            DateTimeOffset timeStamp = _timeStampParser.Parse(content);
            decimal value = _currentLoadParser.Parse(content);

            return new Observation(timeStamp, value);
        }
    }
}