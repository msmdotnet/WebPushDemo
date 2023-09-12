namespace WebPush.Blazor.Services;
public class WebPushService : IAsyncDisposable
{
    readonly Lazy<Task<IJSObjectReference>> ModuleTask;
    readonly IOptions<WebPushNotificationOptions> Options;
    readonly ILogger<WebPushService> Logger;
    public WebPushService(IJSRuntime jsRuntime,
        IOptions<WebPushNotificationOptions> options,
        ILogger<WebPushService> logger)
    {
        Options = options;
        Logger = logger;
        ModuleTask = new(() =>
        jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", $"./_content/{GetType().Assembly.GetName().Name}/js/pushNotification.js")
        .AsTask()
            );
    }

    public async Task<SubscriptionInfo> GetSubscriptionAsync()
    {
        SubscriptionInfo Subscription = default;
        try
        {
            var Module = await ModuleTask.Value;
            Subscription = await Module.InvokeAsync<SubscriptionInfo>(
                "getSubscription", Options.Value.ServerPublicKey);
        }
        catch (Exception ex)
        {
            Logger.LogError("ERROR: {0}", ex.Message);
        }
        return Subscription;
    }

    public async ValueTask DisposeAsync()
    {
        if (ModuleTask.IsValueCreated)
        {
            var Module = await ModuleTask.Value;
            await Module.DisposeAsync();
        }
    }
}
