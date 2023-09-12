namespace WebPush.Blazor.Models;
public class SubscriptionInfo
{
    public string Endpoint { get; set; }
    public string P256dh { get; set; }
    public string Auth { get; set; }
    public bool IsNewSubscription { get; set; }
}
