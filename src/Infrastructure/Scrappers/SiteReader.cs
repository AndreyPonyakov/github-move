using System.IO;
using System.Net;
using System.Text;
using Infrastructure.Exceptions;

public class SiteReader : ISiteReader
{
    private readonly string _url;

    public SiteReader(string url)
    {
        _url = url;
    }

    public string GetHtmlContent()
    {
        var request = (HttpWebRequest)WebRequest.Create(_url);
        request.Method = "GET";

        var response = (HttpWebResponse)request.GetResponse();
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new ScrapperException("Failed to get html content");
        }

        var responseStream = response.GetResponseStream();

        using var ms = new MemoryStream();
        responseStream?.CopyTo(ms);

        return Encoding.UTF8.GetString(ms.ToArray());
    }
}