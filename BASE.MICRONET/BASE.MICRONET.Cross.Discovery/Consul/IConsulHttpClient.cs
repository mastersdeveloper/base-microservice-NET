using System.Threading.Tasks;

namespace BASE.MICRONET.Cross.Discovery.Consul
{
    public interface IConsulHttpClient
    {
        Task<T> GetAsync<T>(string requestUri);
    }
}

