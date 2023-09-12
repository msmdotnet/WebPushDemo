namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddPushNotificationService(
        this IServiceCollection services,
        Action<WebPushNotificationOptions> configure)
    {
        services.AddScoped<WebPushService>();
        services.AddOptions<WebPushNotificationOptions>()
            .Configure(configure);

        return services;
    }
}

