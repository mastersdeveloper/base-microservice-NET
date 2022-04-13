using BASE.MICRONET.Cross.Discovery.Consul;
using BASE.MICRONET.Cross.Discovery.Mvc;
using BASE.MICRONET.Cross.Http.Dir;
using BASE.MICRONET.Cross.Tracing.Dir;
using BASE.MICRONET.Deposit.Messages.CommandHandlers;
using BASE.MICRONET.Deposit.Messages.Commands;
using BASE.MICRONET.Deposit.Repositories;
using BASE.MICRONET.Deposit.Services;
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
using System.Reflection;

namespace BASE.MICRONET.Deposit
{
    //Polly: tu puedes manejar politicas de reintentos 
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
             options =>
             {
                 options.UseNpgsql(Configuration["postgres:cn"]);
             });

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAccountService, AccountService>();

            /*Start RabbitMQ*/
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddRabbitMQ();
            services.AddTransient<IRequestHandler<TransactionCreateCommand, bool>, TransactionCommandHandler>();
            services.AddTransient<IRequestHandler<NotificationCreateCommand, bool>, NotificationCommandHandler>();
            /*End RabbitMQ*/

            services.AddProxyHttp();

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

            //Cuando la aplicacion se apaga se retira el registro de su base de datos de Consul             
            var serviceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId);
            });
        }
    }
}
