namespace WebPushDemo.AppServer.Services;

public class PushNotificationService
{
    readonly IServiceScopeFactory ServiceScopeFactory;
    readonly VapidInfoOptions VapidInfoOptions;
    readonly string DataFileName;

    public PushNotificationService(IWebHostEnvironment environment,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<VapidInfoOptions> vapidInfoOptions)
    {
        ServiceScopeFactory = serviceScopeFactory;
        VapidInfoOptions = vapidInfoOptions.Value;
        DataFileName = $"{environment.ContentRootPath}\\Data\\Data.json";
    }

    public async Task Subscribe(WebPushSubscription subscription)
    {
        using var FS = new FileStream(DataFileName, FileMode.Create);
        await JsonSerializer.SerializeAsync(FS, subscription);
    }

    public void SendExampleNotification()
    {
        Task.Run(async () =>
        {
            var Delay = 45000;
            using var Scope = ServiceScopeFactory.CreateScope();
            var WebPushService = Scope.ServiceProvider
            .GetRequiredService<WebPushService>();
            var Logger = Scope.ServiceProvider
            .GetRequiredService<ILogger<PushNotificationService>>();

            var Stream = new FileStream(DataFileName, FileMode.Open, FileAccess.Read);
            var SubscriptionData = await JsonSerializer
            .DeserializeAsync<WebPushSubscription>(Stream);
            var Subscription = new SubscriptionInfo(SubscriptionData.Endpoint,
                SubscriptionData.P256dh, SubscriptionData.Auth);
            Stream.Dispose();

            VapidInfo VapidInfo = new VapidInfo(
                VapidInfoOptions.Subject, VapidInfoOptions.PublicKey,
                VapidInfoOptions.PrivateKey);

            await SendNotificationAsync(WebPushService, Logger,
                Subscription, VapidInfo, "When I grow up, I want to be a watermelon");
            await Task.Delay(Delay);

            await SendNotificationAsync(WebPushService, Logger,
                Subscription, VapidInfo, "When I grow up, I want to be an apple");
            await Task.Delay(Delay);

            await SendNotificationAsync(WebPushService, Logger,
                Subscription, VapidInfo, "When I grow up, I want to be a pineapple");
            await Task.Delay(Delay);

        });
    }

    static async Task SendNotificationAsync(WebPushService webPushService,
        ILogger<PushNotificationService> logger,
        SubscriptionInfo subscription, VapidInfo vapidInfo, string message)
    {
        var Payload = new
        {
            message = message,
            url = "/counter"
        };

        var SerializedPayload = JsonSerializer.Serialize(Payload);

        try
        {
            await webPushService.SendNotificationAsync(subscription,
                SerializedPayload, vapidInfo);
            logger.LogInformation("*** Send '{0}' notification. ***", message);
        }
        catch (Exception ex)
        {
            logger.LogError("{0}", ex.Message);
        }
    }
}
