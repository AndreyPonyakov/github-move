using Infrastructure.Interfaces;

namespace Infrastructure.Storages
{
    public class AngaraDataCatalogStorage : IStorage
    {
        private string mainlandTimeSeriesName;

        public AngaraDataCatalogStorage(string mainlandTimeSeriesName)
        {
            this.mainlandTimeSeriesName = mainlandTimeSeriesName;
        }
    }
}