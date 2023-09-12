namespace WebPush.Server.Models;
internal class WebPushBodyBuilderResult
{
    public byte[] PublicKey { get; set; }
    public byte[] Payload { get; set; }
    public byte[] Salt { get; set; }
}
