namespace WebPush.Server.Cryptography;
internal class AESGCM
{
    public static byte[] Encrypt(byte[] nonce, byte[] key, byte[] message)
    {
        using AesGcm Aes = new AesGcm(key);

        var CipherText = new byte[message.Length];
        var Tag = new byte[AesGcm.TagByteSizes.MaxSize];

        Aes.Encrypt(nonce, message, CipherText, Tag);

        byte[] Result = new byte[CipherText.Length + Tag.Length];
        Buffer.BlockCopy(CipherText, 0, Result, 0, CipherText.Length);
        Buffer.BlockCopy(Tag, 0, Result, CipherText.Length, Tag.Length);
        return Result;
    }
}
