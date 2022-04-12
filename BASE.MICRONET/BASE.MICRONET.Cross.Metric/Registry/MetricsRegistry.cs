using App.Metrics;
using App.Metrics.Counter;
using Microsoft.Extensions.Configuration;

namespace BASE.MICRONET.Cross.Metric.Registry
{
    public class MetricsRegistry : IMetricsRegistry
    {

        private readonly IMetricsRoot metricsRoot;
        private readonly CounterOptions findQueries;

        public MetricsRegistry(IMetricsRoot metricsRoot, IConfiguration configuration)
        {
            this.metricsRoot = metricsRoot;
            this.findQueries = new CounterOptions { Name = configuration["app:name"] };
        }

        public void IncrementFindQuery()
        {
            metricsRoot.Measure.Counter.Increment(findQueries);
        }
    }
}
