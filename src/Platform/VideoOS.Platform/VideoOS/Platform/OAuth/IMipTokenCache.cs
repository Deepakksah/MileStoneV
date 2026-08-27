using System;

namespace VideoOS.Platform.OAuth;

public interface IMipTokenCache : IDisposable
{
	string Token { get; }

	bool IsTokenExpired();

	string GetClaimValue(string key);
}
