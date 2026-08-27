using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using VideoOS.IM.Server.Service.Annotations;

namespace VideoOS.IM.Server.Service.OidcTokens;

public interface IS2STokenService
{
	void InitializeTokenCache();

	[NotNull]
	Task<string> GetAccessTokenAsync();

	[NotNull]
	Task<JwtSecurityToken> GetAccessTokenAsTokenAsync();
}
