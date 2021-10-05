using LocationService.Application.Interfaces;
using LocationService.Application.Settings;
using LocationService.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LocationService.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<ExternalServiceSetting>(_config.GetSection("ExternalServiceSetting"));
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IHttpClientService, HttpClientService>();
        }
    }
}
