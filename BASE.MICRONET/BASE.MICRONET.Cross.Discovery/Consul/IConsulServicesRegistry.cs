using System.Threading.Tasks;
using Consul;

namespace BASE.MICRONET.Cross.Discovery.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<AgentService> GetAsync(string name);
    }
}