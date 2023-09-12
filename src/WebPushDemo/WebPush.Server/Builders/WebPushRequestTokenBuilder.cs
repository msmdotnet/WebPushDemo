namespace WebPush.Server.Builders;
internal class WebPushRequestTokenBuilder
{
    public static string Build(SubscriptionInfo subscription, VapidInfo vapidInfo)
    {
        var Header = new Dictionary<string, string>
        {
            { "typ", "JWT"},
            { "alg", "ES256" }
        };

        var Uri = new Uri(subscription.Endpoint);
        var Audience = $"{Uri.Scheme}://{Uri.Host}";

        //long Expiration = ((DateTimeOffset)DateTime.UtcNow.AddHours(12))
        //    .ToUnixTimeSeconds() ;

        long Expiration = DateTimeOffset.UtcNow.AddHours(12)
            .ToUnixTimeSeconds();

        var Payload = new Dictionary<string, object>
        {
            { "aud", Audience},
            { "exp", Expiration},
            { "sub", vapidInfo.Subject }
        };

        var EncodedHeader = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(Header)).ToBase64UrlString();
        var EncodedPayload = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(Payload)).ToBase64UrlString();

        var UnsignedToken = $"{EncodedHeader}.{EncodedPayload}";

        var Signature = ECDSA.Sign(UnsignedToken,
            vapidInfo.PrivateKey.ToBytesFromBase64Url()).ToBase64UrlString();

        return $"{UnsignedToken}.{Signature}";
    }
}
