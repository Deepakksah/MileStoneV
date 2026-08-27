using System.Numerics;

namespace VideoOS.Mobile.Portable.Encryption;

public class DHKeyPair
{
	private readonly BigInteger _x;

	private readonly BigInteger _y;

	public DHKeyPair(BigInteger privateKey, BigInteger publicKey)
	{
		_x = privateKey;
		_y = publicKey;
	}

	public BigInteger GetPrivateKey()
	{
		return _x;
	}

	public BigInteger GetPublicKey()
	{
		return _y;
	}
}
