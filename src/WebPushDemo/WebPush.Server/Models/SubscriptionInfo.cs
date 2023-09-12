namespace WebPush.Server.Models;
public class SubscriptionInfo
{
    public string Endpoint { get; }
    public string P256DH { get; }
    public string Auth { get; }

    public SubscriptionInfo(string endpoint, string p256DH, string auth)
    {
        Endpoint = endpoint;
        P256DH = p256DH;
        Auth = auth;
    }
}
