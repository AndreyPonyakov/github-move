namespace Infrastructure.Interfaces
{
    public interface ICurrentLoadParser
    {
        decimal Parse(string content);
    }
}