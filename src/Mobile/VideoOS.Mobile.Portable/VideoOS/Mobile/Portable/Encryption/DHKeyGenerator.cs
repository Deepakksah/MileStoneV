using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Encryption;

public class DHKeyGenerator
{
	private BigInteger P;

	private BigInteger G;

	private static readonly byte[] Skip1024ModulusBytes = new byte[129]
	{
		199, 120, 47, 233, 85, 195, 94, 162, 171, 186,
		38, 25, 34, 75, 137, 189, 124, 156, 159, 65,
		93, 164, 102, 61, 8, 211, 70, 2, 155, 80,
		10, 208, 171, 159, 150, 217, 12, 149, 202, 122,
		41, 7, 200, 25, 3, 196, 134, 214, 114, 10,
		167, 132, 124, 141, 17, 24, 64, 41, 177, 225,
		60, 255, 42, 181, 44, 33, 51, 222, 108, 24,
		29, 163, 67, 87, 23, 124, 253, 52, 211, 56,
		93, 184, 140, 85, 187, 142, 141, 24, 91, 165,
		240, 67, 192, 35, 201, 243, 246, 142, 45, 91,
		124, 28, 179, 136, 124, 15, 29, 69, 13, 56,
		108, 51, 107, 54, 7, 145, 228, 157, 180, 32,
		205, 219, 73, 78, 88, 253, 136, 244, 0
	};

	private static readonly string Skip2048ModulusString = "87A8E61DB4B6663CFFBBD19C651959998CEEF608660DD0F25D2CEED4435E3B00E00DF8F1D61957D4FAF7DF4561B2AA3016C3D91134096FAA3BF4296D830E9A7C209E0C6497517ABD5A8A9D306BCF67ED91F9E6725B4758C022E0B1EF4275BF7B6C5BFC11D45F9088B941F54EB1E59BB8BC39A0BF12307F5C4FDB70C581B23F76B63ACAE1CAA6B7902D52526735488A0EF13C6D9A51BFA4AB3AD8347796524D8EF6A167B5A41825D967E144E5140564251CCACB83E6B486F6B3CA3F7971506026C0B857F689962856DED4010ABD0BE621C3A3960A54E710C375F26375D7014103A4B54330C198AF126116D2276E11715F693877FAD7EF09CADB094AE91E1A1597";

	private static readonly BigInteger Skip1024Modulus = new BigInteger(Skip1024ModulusBytes);

	private static readonly BigInteger Skip2048Modulus = new BigInteger(StringToByteArray(Skip2048ModulusString));

	private static readonly BigInteger Skip1024Base = new BigInteger(2);

	private const int KeyLenExponent = 28;

	private byte[] _keyBytes;

	public static byte[] StringToByteArray(string hex)
	{
		List<byte> list = (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToList();
		list.Reverse();
		list.Add(0);
		return list.ToArray();
	}

	public void Initialize(PrimeLength primeLength)
	{
		switch (primeLength)
		{
		case PrimeLength.Prime1024:
			P = Skip1024Modulus;
			break;
		case PrimeLength.Prime2048:
			P = Skip2048Modulus;
			break;
		default:
			P = Skip1024Modulus;
			break;
		}
		G = Skip1024Base;
	}

	public void Initialize(byte[] p, int g)
	{
		P = new BigInteger(p);
		G = new BigInteger(g);
	}

	public DHKeyPair GenerateKeyPair()
	{
		RandomGenerator randomGenerator = new RandomGenerator();
		byte[] array = new byte[28];
		randomGenerator.GetBytes(array);
		array[27] &= 127;
		BigInteger bigInteger = new BigInteger(array);
		if (bigInteger.Sign == -1)
		{
			bigInteger = BigInteger.Negate(bigInteger);
		}
		BigInteger publicKey = BigInteger.ModPow(G, bigInteger, P);
		return new DHKeyPair(bigInteger, publicKey);
	}

	protected BigInteger GenerateSecretKey(BigInteger publicKey, BigInteger privateKey)
	{
		return BigInteger.ModPow(publicKey, privateKey, P);
	}

	public static string ByteArrayToString(byte[] ba)
	{
		StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
		foreach (byte b in ba)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public byte[] GetSecretKeyBytes(int keysize, BigInteger publicKey, BigInteger privateKey)
	{
		_keyBytes = GenerateSecretKey(publicKey, privateKey).ToByteArray();
		int num = _keyBytes.Length;
		byte[] array = new byte[keysize];
		int num2 = 0;
		for (int i = 0; i < keysize; i++)
		{
			array[i] = _keyBytes[num2];
			num2++;
			if (num2 >= num)
			{
				num2 = 0;
			}
		}
		return array;
	}

	public byte[] GetFullSecretKeyBytes()
	{
		int num = ((_keyBytes.Length > 1 && _keyBytes[_keyBytes.Length - 1] == 0) ? (_keyBytes.Length - 1) : _keyBytes.Length);
		byte[] array = new byte[num];
		Array.Copy(_keyBytes, array, num);
		return array;
	}
}
