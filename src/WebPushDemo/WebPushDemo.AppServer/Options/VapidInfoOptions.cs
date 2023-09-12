namespace WebPushDemo.AppServer.Options;

public class VapidInfoOptions
{
    public const string SectionKey = "VapidInfo";
    public string Subject { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}
