using System.Threading.Tasks;

public interface ISiteReader
{
    string GetHtmlContent();
    Task<string> GetHtmlContentAsync();
}