using Blazored.Modal.Configuration;
using Blazored.Modal.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blazored.Modal;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazoredModal(this IServiceCollection services, Action<Settings>? configure = null)
    {
        services.AddScoped<IModalService, ModalService>();
        OptionsBuilder<Settings> optionsBuilder = services.AddOptions<Settings>();
        optionsBuilder.Configure(c => c.JsPath = null);
        if (configure != null)
            optionsBuilder.Configure(configure);
        optionsBuilder.PostConfigure<IServiceProvider>((settings, serviceProvider) =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            configuration?.GetSection("BlazoredModal").Bind(settings);
        });
        return services;
    }
}

