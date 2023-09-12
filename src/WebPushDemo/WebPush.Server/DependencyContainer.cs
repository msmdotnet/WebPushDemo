namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddWebPushService(
        this IServiceCollection services)
    {
        services.AddScoped<WebPushService>();
        services.AddHttpClient<WebPushService>();

        return services;
    }
}

