using BASE.MICRONET.Cross.Discovery.Fabio;
using BASE.MICRONET.Cross.Discovery.Mvc;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using BASE.MICRONET.Cross.Discovery.Dir;

namespace BASE.MICRONET.Cross.Discovery.Consul
{
    public static class Extensions
    {
        private static readonly string ConsulSectionName = "consul";
        private static readonly string FabioSectionName = "fabio";

        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddSingleton<IServiceId, ServiceId>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var options = configuration.GetOptions<ConsulOptions>(ConsulSectionName);
            services.Configure<ConsulOptions>(configuration.GetSection(ConsulSectionName));
            services.Configure<FabioOptions>(configuration.GetSection(FabioSectionName));
            services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();
            services.AddTransient<ConsulServiceDiscoveryMessageHandler>();
            services.AddHttpClient<IConsulHttpClient, ConsulHttpClient>()
                .AddHttpMessageHandler<ConsulServiceDiscoveryMessageHandler>();

            return services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(options.Url))
                {
                    cfg.Address = new Uri(options.Url);
                }
            }));
        }

        //Returns unique service ID used for removing the service from registry.
        public static string UseConsul(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var consulOptions = scope.ServiceProvider.GetService<IOptions<ConsulOptions>>();
                var fabioOptions = scope.ServiceProvider.GetService<IOptions<FabioOptions>>();
                var enabled = consulOptions.Value.Enabled;
                var consulEnabled = Environment.GetEnvironmentVariable("CONSUL_ENABLED")?.ToLowerInvariant();
                var aspnetcoreEnviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLowerInvariant();

                if (!string.IsNullOrWhiteSpace(consulEnabled))
                {
                    enabled = consulEnabled == "true" || consulEnabled == "1";
                }

                if (!enabled)
                {
                    return string.Empty;
                }

                string address = "";
                int portServer = 0;
                Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {aspnetcoreEnviroment}");
                if (aspnetcoreEnviroment == "docker")
                {
                    var hostName = Dns.GetHostName();
                    var ip = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    address = ip.ToString();
                }
                else
                {
                    if (aspnetcoreEnviroment == "local")
                    {
                        address = "host.docker.internal";
                    }
                    else
                    {
                        var server = scope.ServiceProvider.GetService<IServer>();
                        var features = server.Features;
                        var addresses = features.Get<IServerAddressesFeature>();
                        var url = addresses.Addresses.First();
                        var serviceFull = new Uri(url);

                        address = serviceFull.Host;
                        portServer = serviceFull.Port;
                    }
                }
                Console.WriteLine($"Address: {address}");
                //var address = consulOptions.Value.Address;

                if (string.IsNullOrWhiteSpace(address))
                {
                    throw new ArgumentException("Consul address can not be empty.",
                        nameof(consulOptions.Value.PingEndpoint));
                }

                var uniqueId = scope.ServiceProvider.GetService<IServiceId>().Id;
                var client = scope.ServiceProvider.GetService<IConsulClient>();
                var serviceName = consulOptions.Value.Service;
                var serviceId = $"{serviceName}:{uniqueId}";

                //var port = consulOptions.Value.Port;
                var port = (portServer == 0 ? consulOptions.Value.Port : portServer);
                Console.WriteLine($"Port: {port}");

                var pingEndpoint = consulOptions.Value.PingEndpoint;
                var pingInterval = consulOptions.Value.PingInterval <= 0 ? 5 : consulOptions.Value.PingInterval;
                var removeAfterInterval =
                    consulOptions.Value.RemoveAfterInterval <= 0 ? 10 : consulOptions.Value.RemoveAfterInterval;
                var registration = new AgentServiceRegistration
                {
                    Name = serviceName,
                    ID = serviceId,
                    Address = address,
                    Port = port,
                    Tags = fabioOptions.Value.Enabled ? GetFabioTags(serviceName, fabioOptions.Value.Service) : null
                };
                if (consulOptions.Value.PingEnabled || fabioOptions.Value.Enabled)
                {
                    var scheme = address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                        ? string.Empty
                        : "http://";
                    var check = new AgentServiceCheck
                    {
                        Interval = TimeSpan.FromSeconds(pingInterval),
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(removeAfterInterval),
                        HTTP = $"{scheme}{address}{(port > 0 ? $":{port}" : string.Empty)}/{pingEndpoint}"
                    };
                    registration.Checks = new[] { check };
                }

                client.Agent.ServiceRegister(registration);

                return serviceId;
            }
        }

        private static string[] GetFabioTags(string consulService, string fabioService)
        {
            var service = (string.IsNullOrWhiteSpace(fabioService) ? consulService : fabioService)
                .ToLowerInvariant();

            return new[] { $"urlprefix-/{service} strip=/{service}" };
        }
    }
}