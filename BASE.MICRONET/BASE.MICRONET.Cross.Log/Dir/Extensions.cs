using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BASE.MICRONET.Cross.Log.Dir
{
    public static class Extensions
    {
        private static readonly string SectionName = "logseq";
        public static void UseLogSeq(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {

                IConfiguration configuration = scope.ServiceProvider.GetService<IConfiguration>();

                var options = configuration.GetOptions<LogOptions>(SectionName);

                if (options.Enabled)
                {
                    var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();
                    loggerFactory.AddSeq(options.Url, apiKey: options.Token);
                }
            }
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
    }
}
