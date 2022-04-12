using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Formatters.Prometheus;
using BASE.MICRONET.Cross.Metric.Dir;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BASE.MICRONET.Cross.Metric.Metrics
{
    public static class Extensions
    {
        private static bool _initialized;
        private static readonly string SectionName = "metrics";

        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<MetricsOptions>(SectionName);

            if (options.Enabled)
            {
                /*Start - Metrics*/
                // If using Kestrel:
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });

                // If using IIS:
                services.Configure<IISServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });
                /*End - Metrics*/
            }
            return services;
        }

        public static IWebHostBuilder UseAppMetrics(this IWebHostBuilder webHostBuilder)
        {
            if (_initialized)
            {
                return webHostBuilder;
            }

            return webHostBuilder
                .ConfigureMetricsWithDefaults((context, builder) =>
                {
                    var metricsOptions = context.Configuration.GetOptions<MetricsOptions>(SectionName);
                    if (!metricsOptions.Enabled)
                    {
                        return;
                    }

                    _initialized = true;
                    builder.Configuration.Configure(cfg =>
                    {
                        var tags = metricsOptions.Tags;
                        if (tags == null)
                        {
                            return;
                        }

                        tags.TryGetValue("app", out var app);
                        tags.TryGetValue("env", out var env);
                        tags.TryGetValue("server", out var server);
                        cfg.AddAppTag(string.IsNullOrWhiteSpace(app) ? null : app);
                        cfg.AddEnvTag(string.IsNullOrWhiteSpace(env) ? null : env);
                        cfg.AddServerTag(string.IsNullOrWhiteSpace(server) ? null : server);
                        foreach (var tag in tags)
                        {
                            if (!cfg.GlobalTags.ContainsKey(tag.Key))
                            {
                                cfg.GlobalTags.Add(tag.Key, tag.Value);
                            }
                        }
                    }
                    );
                })
                .UseHealth()
                .UseHealthEndpoints()
                .UseMetricsWebTracking()
                .UseMetrics((context, options) =>
                {
                    var metricsOptions = context.Configuration.GetOptions<MetricsOptions>(SectionName);
                    if (!metricsOptions.Enabled)
                    {
                        return;
                    }

                    if (!metricsOptions.PrometheusEnabled)
                    {
                        return;
                    }

                    options.EndpointOptions = endpointOptions =>
                    {
                        switch (metricsOptions.PrometheusFormatter?.ToLowerInvariant() ?? string.Empty)
                        {
                            case "protobuf":
                                endpointOptions.MetricsEndpointOutputFormatter =
                                    new MetricsPrometheusProtobufOutputFormatter();
                                break;
                            default:
                                endpointOptions.MetricsEndpointOutputFormatter =
                                    new MetricsPrometheusTextOutputFormatter();
                                break;
                        }
                    };

                });
        }
    }
}
