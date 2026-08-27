using System;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using VideoOS.IdentityServer.Data;

namespace VideoOS.IdentityServer.Helper;

public static class RsaKeyGenerator
{
	public const int DefaultKeySize = 2048;

	public const int MaximumKeySize = 16384;

	public const int MinimumKeySize = 384;

	public static RsaSecurityKey GenerateRsaSecurityKey(int keySize = 2048)
	{
		if (keySize < 384 || keySize > 16384)
		{
			throw new ArgumentOutOfRangeException("keySize", keySize, $"{"keySize"} cannot be less than {384} or bigger than {16384}, and should be at minimum {2048}.");
		}
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(keySize);
		try
		{
			return new RsaSecurityKey(rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true))
			{
				KeyId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)
			};
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	public static SigningCredentials GetCredentials(SecurityKeyDataModel model)
	{
		RsaSecurityKey key = new RsaSecurityKey(new RSAParameters
		{
			D = model.D,
			DP = model.DP,
			DQ = model.DQ,
			Exponent = model.Exponent,
			InverseQ = model.InverseQ,
			Modulus = model.Modulus,
			P = model.P,
			Q = model.Q
		})
		{
			KeyId = model.KeyId
		};
		return new SigningCredentials(key, "RS256", "http://www.w3.org/2001/04/xmlenc#sha256");
	}
}
