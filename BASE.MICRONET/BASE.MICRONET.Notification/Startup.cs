using BASE.MICRONET.Cross.Discovery.Consul;
using BASE.MICRONET.Cross.Discovery.Mvc;
using BASE.MICRONET.Cross.Event.Dir.Bus;
using BASE.MICRONET.Cross.Tracing.Dir;
using BASE.MICRONET.Notification.Messages.EventHandlers;
using BASE.MICRONET.Notification.Messages.Events;
using BASE.MICRONET.Notification.Repositories;
using BASE.MICRONET.Notification.Services;
using BASE.MICRONETBASE.MICRONET.Cross.Event.Dir;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BASE.MICRONET.Notification
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
                 opt.UseMySQL(Configuration["mariadb:cn"]);
             });

            services.AddScoped<INotificationService, NotificationService>();

            /*Start - RabbitMQ*/
            services.AddMediatR(typeof(Startup));
            services.AddRabbitMQ();

            services.AddTransient<NotificationEventHandler>();
            services.AddTransient<IEventHandler<NotificationCreatedEvent>, NotificationEventHandler>();
            /*End - RabbitMQ*/

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
            eventBus.Subscribe<NotificationCreatedEvent, NotificationEventHandler>();
        }
    }
}
