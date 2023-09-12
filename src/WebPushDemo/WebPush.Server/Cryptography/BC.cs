using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Bouncy = Org.BouncyCastle.Math.EC;

namespace WebPush.Server.Cryptography;
internal class BC
{
    public static byte[] ComputeSecret(byte[] bobPrivateKey, byte[] alicePublicKey)
    {
        BigInteger D = new BigInteger(1, bobPrivateKey);
        Bouncy.ECPoint PublicPoint =
            GetPublicPointFromPublicKeyBytes(alicePublicKey);
        Bouncy.ECPoint OtherPointInTheCurve = PublicPoint.Multiply(D);
        OtherPointInTheCurve = OtherPointInTheCurve.Normalize();

        return OtherPointInTheCurve.AffineXCoord.ToBigInteger()
            .ToByteArrayUnsigned();
    }

    static Bouncy.ECPoint GetPublicPointFromPublicKeyBytes(byte[] pubKey)
    {
        X9ECParameters CurveParameters = ECNamedCurveTable.GetByName("secp256r1");
        ECDomainParameters DomainParameters = new ECDomainParameters(
            CurveParameters.Curve,
            CurveParameters.G, CurveParameters.N, CurveParameters.H);

        return DomainParameters.Curve.CreatePoint(
            new BigInteger(1, pubKey[1..33]),
            new BigInteger(1, pubKey[33..]));
    }
}
