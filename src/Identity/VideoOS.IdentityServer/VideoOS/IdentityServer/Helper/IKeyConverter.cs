using System.Collections.Generic;
using IdentityModel.Jwk;
using Microsoft.IdentityModel.Tokens;

namespace VideoOS.IdentityServer.Helper;

public interface IKeyConverter
{
	IEnumerable<SecurityKey> ConvertJsonWebKeyToSecurityKey(List<IdentityModel.Jwk.JsonWebKey> webKeys);
}
