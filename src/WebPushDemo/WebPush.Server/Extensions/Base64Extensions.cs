namespace WebPush.Server.Extensions;
internal static class Base64Extensions
{
    public static byte[] ToBytesFromBase64Url(this string base64UrlValue) =>
        Base64UrlEncoder.DecodeBytes(base64UrlValue);

    public static string ToBase64UrlString(this byte[] bytes) =>
        Base64UrlEncoder.Encode(bytes);

    public static byte[] ToBytesFromBase64(this string base64Value) =>
        Convert.FromBase64String(base64Value);
}
