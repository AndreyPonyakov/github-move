using System.Threading.Tasks;
using Domain.Entites;

namespace Infrastructure.Interfaces
{
    public interface IScrapper
    {
        public string Name { get; }
        Observation GetData();
        Task<Observation> GetDataAsync();
    }
}