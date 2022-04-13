using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BASE.MICRONET.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //Al agregar aqui estamos indicando que existe otro proveedor de configuracion 
                    webBuilder.ConfigureAppConfiguration((host, config) =>
                    {
                        config
                            .AddJsonFile($"ocelot.{host.HostingEnvironment.EnvironmentName}.json", true, true)
                            .AddJsonFile("ocelot.json", false, true);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
