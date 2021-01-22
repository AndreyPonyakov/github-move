using System;
using Domain.Entites;

namespace Infrastructure.Interfaces
{
    public interface IStorage : IDisposable
    {
        void Save(Observation observation);
    }
}