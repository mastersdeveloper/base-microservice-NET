using BASE.MICRONET.Cross.Cache.Dir;
using BASE.MICRONET.Cross.Discovery.Consul;
using BASE.MICRONET.Cross.Discovery.Mvc;
using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.History.Messages.EventHandlers;
using BASE.MICRONET.History.Messages.Events;
using BASE.MICRONET.History.Repositories;
using BASE.MICRONET.History.Services;
using BASE.MICRONETBASE.MICRONET.Cross.Event.Dir;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BASE.MICRONET.History
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

            services.Configure<Mongosettings>(opt =>
            {
                opt.Connection = Configuration.GetSection("mongo:cn").Value;
                opt.DatabaseName = Configuration.GetSection("mongo:database").Value;
            });
            services.AddScoped<IHistoryService, HistoryService>();
            services.AddScoped<IMongoBookDBContext, MongoBookDBContext>();

            /*Start - RabbitMQ*/
            services.AddMediatR(typeof(Startup));
            services.AddRabbitMQ();

            services.AddTransient<TransactionEventHandler>();
            services.AddTransient<IEventHandler<TransactionCreatedEvent>, TransactionEventHandler>();
            /*End - RabbitMQ*/

            /*Start - Consul*/
            services.AddSingleton<IServiceId, ServiceId>();//Genera un Guid, para identificar al registro
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //Comunicacion Http
            services.AddConsul();//permite implementar toda la funcionalidad, se registra en Consul 
            /*End - Consul*/

            services.AddRedis();
            services.AddSingleton<IExtensionCache, ExtensionCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            ConfigureEventBus(app);

            //Cuando la aplicacion se apaga se retira el registro de su base de datos de Consul             
            var serviceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId);
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransactionCreatedEvent, TransactionEventHandler>();
        }
    }
}
