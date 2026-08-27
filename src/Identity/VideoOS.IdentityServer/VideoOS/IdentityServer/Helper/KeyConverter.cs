using System.Collections.Generic;
using System.Security.Cryptography;
using IdentityModel;
using IdentityModel.Jwk;
using Microsoft.IdentityModel.Tokens;

namespace VideoOS.IdentityServer.Helper;

public class KeyConverter : IKeyConverter
{
	public IEnumerable<SecurityKey> ConvertJsonWebKeyToSecurityKey(List<IdentityModel.Jwk.JsonWebKey> webKeys)
	{
		List<SecurityKey> list = new List<SecurityKey>();
		if (webKeys == null)
		{
			return list;
		}
		foreach (IdentityModel.Jwk.JsonWebKey webKey in webKeys)
		{
			byte[] exponent = Base64Url.Decode(webKey.E);
			byte[] modulus = Base64Url.Decode(webKey.N);
			RsaSecurityKey item = new RsaSecurityKey(new RSAParameters
			{
				Exponent = exponent,
				Modulus = modulus
			})
			{
				KeyId = webKey.Kid
			};
			list.Add(item);
		}
		return list;
	}
}
