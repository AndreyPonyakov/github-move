namespace Infrastructure.Interfaces
{
    public interface IScrapper
    {
        public string Name { get; }
        decimal GetData();
    }
}