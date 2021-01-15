namespace Infrastructure.Interfaces
{
    public interface IStorage
    {
        void Save(global::Domain.Entites.Observation observation);
    }
}