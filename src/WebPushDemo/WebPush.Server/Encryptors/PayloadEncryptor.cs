namespace WebPush.Server.Encryptors;
internal class PayloadEncryptor
{
    public static WebPushBodyBuilderResult Encrypt(string subscriptionP256DH,
        string subscriptionAuth, string payload)
    {
        var P256DHBytes = subscriptionP256DH.ToBytesFromBase64();
        var AuthBytes = subscriptionAuth.ToBytesFromBase64();
        var PayloadBytes = Encoding.UTF8.GetBytes(payload);

        var Salt = GenerateSalt(16);

        var LocalKeysCurve = new ECDiffieHellmanP256();

        var SharedSecret = LocalKeysCurve.GetSharedSecret(P256DHBytes);

        var PseudoRandomKey = DoHKDF(AuthBytes, SharedSecret,
            Encoding.UTF8.GetBytes("Content-Encoding: auth\0"), 32);

        var ServerPublicKey = LocalKeysCurve.PublicKey;

        var Context = GetContext(P256DHBytes, ServerPublicKey);

        var NonceInfo = CreateContentEncoding("nonce", Context);
        var ContentEncryptionKeyInfo = CreateContentEncoding("aesgcm", Context);

        var Nonce = DoHKDF(Salt, PseudoRandomKey, NonceInfo, 12);
        var ContentEncryptionKey = DoHKDF(Salt, PseudoRandomKey,
            ContentEncryptionKeyInfo, 16);

        var Input = AddPaddingToInput(PayloadBytes);

        var EncryptedPayload = AESGCM.Encrypt(Nonce, ContentEncryptionKey, Input);

        return new WebPushBodyBuilderResult
        {
            Salt = Salt,
            Payload = EncryptedPayload,
            PublicKey = ServerPublicKey
        };
    }

    static byte[] GenerateSalt(int length)
    {
        var Salt = new byte[length];
        using var Rng = RandomNumberGenerator.Create();
        Rng.GetNonZeroBytes(Salt);
        return Salt;
    }

    public static byte[] DoHKDF(byte[] salt, byte[] ikm, byte[] info, int length) =>
        HKDF.DeriveKey(HashAlgorithmName.SHA256, ikm, length, salt, info);

    public static byte[] ConvertInt(int number)
    {
        var Output = BitConverter.GetBytes(Convert.ToUInt16(number));
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(Output);
        }
        return Output;
    }
    public static byte[] GetContext(byte[] subscriptionP256dh,
        byte[] localPublicKey)
    {
        var Context = new List<byte>();
        Context.AddRange(Encoding.UTF8.GetBytes("P-256\0"));
        Context.AddRange(ConvertInt(subscriptionP256dh.Length));
        Context.AddRange(subscriptionP256dh);

        Context.AddRange(ConvertInt(localPublicKey.Length));
        Context.AddRange(localPublicKey);

        return Context.ToArray();
    }

    public static byte[] CreateContentEncoding(string contentEncoding,
        byte[] context)
    {
        var Result = new List<Byte>();

        Result.AddRange(
            Encoding.UTF8.GetBytes($"Content-Encoding: {contentEncoding}\0"));
        Result.AddRange(context);
        return Result.ToArray();
    }

    static byte[] AddPaddingToInput(byte[] data)
    {
        var Input = new byte[2 + data.Length];
        Buffer.BlockCopy(data, 0, Input, 2, data.Length);
        return Input;
    }
}

