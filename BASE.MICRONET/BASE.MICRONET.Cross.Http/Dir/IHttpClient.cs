using System.Net.Http;
using System.Threading.Tasks;

namespace BASE.MICRONET.Cross.Http.Dir
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);

        Task<HttpResponseMessage> PostAsync<T>(string uri, T item);
    }
}
