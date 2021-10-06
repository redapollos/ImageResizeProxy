using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RainstormTech.Storm.ImageProxy;
using Microsoft.Azure.WebJobs.Host.Bindings;
using RainstormTech.Storm.ImageProxy.Extensions;
using RainstormTech.Storm.ImageProxy.Options;


[assembly: FunctionsStartup(typeof(Startup))]

namespace RainstormTech.Storm.ImageProxy
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOptions<ImageResizerOptions>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("ImageResizer").Bind(settings));

            builder.Services
                .AddOptions<ClientCacheOptions>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("ClientCache").Bind(settings));

            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IImageResizerService, ImageResizerService>();

            builder.Services.FixConfiguration();
        }
    }
}
