namespace WebPush.Server.Cryptography;
internal class ECDiffieHellmanP256
{
    public ECDiffieHellman EcDiffieHellman { get; }
    public byte[] PrivateKey { get; }
    public byte[] PublicKey { get; }

    public ECDiffieHellmanP256()
    {
        ECDiffieHellman Ecdh = ECDiffieHellman.Create(
            ECCurve.NamedCurves.nistP256);

        PublicKey = GetPublicKey(Ecdh);
        PrivateKey = GetPrivateKey(Ecdh);
        EcDiffieHellman = Ecdh;
    }

    static byte[] GetPrivateKey(ECDiffieHellman ecdh) =>
        ecdh.ExportParameters(true).D;
    static byte[] GetPublicKey(ECDiffieHellman ecdh)
    {
        ECParameters ECParam = ecdh.PublicKey.ExportParameters();
        byte[] XBytes = ECParam.Q.X;
        byte[] YBytes = ECParam.Q.Y;

        var RawPublicKeyBytes = new List<byte>
        {
            4
        };
        RawPublicKeyBytes.AddRange(XBytes);
        RawPublicKeyBytes.AddRange(YBytes);
        return RawPublicKeyBytes.ToArray();
    }

    public byte[] GetSharedSecret(byte[] alicePublicKeyBytes) =>
        BC.ComputeSecret(PrivateKey, alicePublicKeyBytes);
}
