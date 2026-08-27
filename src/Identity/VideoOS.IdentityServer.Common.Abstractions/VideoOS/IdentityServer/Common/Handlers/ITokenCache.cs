using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common.Handlers;

public interface ITokenCache : IDisposable
{
	string UserName { get; }

	Task<JwtSecurityToken> RefreshToken();

	Task<JwtSecurityToken> RefreshToken(CancellationToken cancellationToken);

	bool IsTokenExpired();

	Task Start();

	Task Start(CancellationToken cancellationToken);

	Task Stop();

	string GetClaimValue(string key);
}
