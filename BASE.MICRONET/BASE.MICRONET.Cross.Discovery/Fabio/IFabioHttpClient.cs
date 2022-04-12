using System.Threading.Tasks;

namespace BASE.MICRONET.Cross.Discovery.Fabio
{
    public interface IFabioHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}