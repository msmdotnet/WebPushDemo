namespace WebPush.Server.Models;
public class VapidInfo
{
    public string Subject { get; }
    public string PublicKey { get; }
    public string PrivateKey { get; }

    public VapidInfo(string subject, string publicKey, string privateKey)
    {
        Subject = subject;
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }
}
