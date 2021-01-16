using System;

namespace Infrastructure.Interfaces
{
    public interface IStorage : IDisposable
    {
        void Save(global::Domain.Entites.Observation observation);
    }
}