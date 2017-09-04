using System.Net.Http;
using System.Threading.Tasks;

namespace MyData.NancyApi
{
    public interface IHttpIo
    {
        Task<string> AsyncRequest(HttpMethod method, string url, string json, string contentType);
        string SyncRequest(HttpMethod method, string url, string json, string contentType);
    }
}