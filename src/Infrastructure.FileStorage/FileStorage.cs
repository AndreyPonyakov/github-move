using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;
using Infrastructure.Interfaces;

namespace Infrastructure.FileStorage
{
    public sealed class FileStorage : IStorageAsync
    {
        private string _seriesName;

        public FileStorage(string seriesName)
        {
            _seriesName = seriesName;
        }

        public async Task SaveAsync(Observation observation)
        {
            var timeStamp = observation.TimeStamp.ToString("s");
            await File.AppendAllTextAsync(_seriesName + ".txt", $"{DateTime.UtcNow.ToString("s")}; {timeStamp}; {observation.CurrentLoad} \r\n", Encoding.UTF8); 
        }
    }
}
