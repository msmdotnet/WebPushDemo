namespace WebPush.Server.Services;
public class WebPushService
{
    readonly HttpClient Client;

    public WebPushService(HttpClient client)
    {
        Client = client;
    }

    public async Task SendNotificationAsync(SubscriptionInfo subscription,
        string payload, VapidInfo vapidInfo, CancellationToken cancellationToken = default)
    {
        var Request = WebPushHttpRequestBuilder.Build(subscription, payload, vapidInfo);
        var Response = await Client.SendAsync(Request, cancellationToken);
        if (!Response.IsSuccessStatusCode)
        {
            await HandleResponseError(Response);
        }
    }

    static async Task HandleResponseError(HttpResponseMessage response)
    {
        var ResponseCodeMessage =
            $"Received unexpected response code: {response.StatusCode}";
        switch (response.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                ResponseCodeMessage = "Bad Request";
                break;
            case HttpStatusCode.RequestEntityTooLarge:
                ResponseCodeMessage = "Payload too large";
                break;
            case HttpStatusCode.TooManyRequests:
                ResponseCodeMessage = "Too many request";
                break;
            case HttpStatusCode.NotFound:
            case HttpStatusCode.Gone:
                ResponseCodeMessage = "Subscription no longer valid";
                break;
        }

        string Details = null;
        if (response.Content != null)
        {
            Details = await response.Content.ReadAsStringAsync();
        }

        var Message = string.IsNullOrEmpty(Details)
            ? ResponseCodeMessage
            : $"{ResponseCodeMessage}. Details: {Details}";
        throw new Exception(Message);
    }
}
