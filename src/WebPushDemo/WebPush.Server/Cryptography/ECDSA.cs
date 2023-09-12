namespace WebPush.Server.Cryptography;
internal static class ECDSA
{
    public static byte[] Sign(string valueToSign, byte[] privateKey)
    {
        using ECDsa Ecdsa = ECDsa.Create();
        ECParameters ECParameters = new ECParameters
        {
            D = privateKey,
            Curve = ECCurve.NamedCurves.nistP256
        };

        Ecdsa.ImportParameters(ECParameters);

        var UnsignedTokenBytes = Encoding.UTF8.GetBytes(valueToSign);
        byte[] Signature = Ecdsa.SignData(UnsignedTokenBytes,
            HashAlgorithmName.SHA256);

        return Signature;
    }
}
