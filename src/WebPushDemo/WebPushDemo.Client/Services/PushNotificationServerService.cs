namespace WebPushDemo.Client.Services;

public class PushNotificationServerService
{
    readonly HttpClient Client;
    public PushNotificationServerService(HttpClient client)
    {
        Client = client;
    }

    public async Task<bool> SendSubscription(WebPushSubscription subscription)
    {
        // Enviar P256dh, Auth, Endpoint
        var Response = await Client.PostAsJsonAsync("subscribe", subscription);
        return Response.IsSuccessStatusCode;
    }

    public async Task<bool> RequestExampleNotification()
    {
        var Response = await Client.GetAsync(
            "requestexamplenotification");
        return Response.IsSuccessStatusCode;
    }
}
