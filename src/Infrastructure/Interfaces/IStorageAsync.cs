using System;
using System.Threading.Tasks;
using Domain.Entites;

namespace Infrastructure.Interfaces
{
    public interface IStorageAsync
    {
        Task SaveAsync(Observation observation);
    }
}