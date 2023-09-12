using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebPush.Server.Encryptors;
using WebPush.Server.Models;

namespace WebPush.Server.Builders;
internal class WebPushHttpRequestBuilder
{
    public static HttpRequestMessage Build(SubscriptionInfo subscription, string payload,
        VapidInfo vapidInfo)
    {
        var Request = new HttpRequestMessage(HttpMethod.Post, subscription.Endpoint);

        var WebPushAuthorizationToken = WebPushRequestTokenBuilder.Build(
            subscription, vapidInfo);

        var EncryptedPayload = PayloadEncryptor.Encrypt(subscription.P256DH,
            subscription.Auth, payload);

        Request.Headers.Add("Authorization", $"WebPush {WebPushAuthorizationToken}");
        Request.Headers.Add("Encryption", 
            $"salt={EncryptedPayload.Salt.ToBase64UrlString()}" );
        Request.Headers.Add("Crypto-Key", string.Format("dh={0};p256ecdsa={1}",
            EncryptedPayload.PublicKey.ToBase64UrlString(),
            vapidInfo.PublicKey));
        const int TimeToLiveInSeconds = 604800; //60 * 60 * 24 * 7;
        Request.Headers.Add("TTL", TimeToLiveInSeconds.ToString());

        Request.Content = new ByteArrayContent(EncryptedPayload.Payload);
        Request.Content.Headers.ContentLength = EncryptedPayload.Payload.Length;
        Request.Content.Headers.ContentType =
            new MediaTypeHeaderValue("application/octet-stream");
        Request.Content.Headers.ContentEncoding.Add("aesgcm");

        return Request;
    }
}
