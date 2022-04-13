using BASE.MICRONET.Cross.Discovery.Consul;
using BASE.MICRONET.Cross.Discovery.Mvc;
using BASE.MICRONET.Cross.Log.Dir;
using BASE.MICRONET.Cross.Token.Dir;
using BASE.MICRONET.Cross.Tracing.Dir;
using BASE.MICRONET.Security.Repositories;
using BASE.MICRONET.Security.Services;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BASE.MICRONET.Security
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ContextDatabase>(
                opt =>
                {
                    opt.UseMySQL(Configuration["cn:mysql"]);//aca se revirtio el orden que se establecio en Nacos, en Nacos se establecio la cadena de conexion
                });

            services.AddScoped<IAccessService, AccessService>();

            services.Configure<JwtOptions>(Configuration.GetSection("jwt"));

            /*Start - Consul*/
            services.AddSingleton<IServiceId, ServiceId>();//Genera un Guid, para identificar al registro
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //Comunicacion Http
            services.AddConsul();//permite implementar toda la funcionalidad, se registra en Consul 
            /*End - Consul*/

            /*Start - Tracer distributed*/
            services.AddJaeger();
            services.AddOpenTracing();
            /*End - Tracer distributed*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        //IHostApplicationLifetime: interfaz que me permite sobre el ciclo de vida de la aplicacion 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime, IConsulClient consulClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Cuando la aplicacion se apaga se retira el registro de su base de datos de Consul             
            var serviceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId);
            });

            //Log Seq
            app.UseLogSeq();
        }
    }
}
