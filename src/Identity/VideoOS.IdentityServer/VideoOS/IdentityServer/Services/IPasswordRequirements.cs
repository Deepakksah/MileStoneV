using VideoOS.IdentityServer.Configuration;

namespace VideoOS.IdentityServer.Services;

public interface IPasswordRequirements
{
	PasswordSettings Settings { get; }

	string Pattern { get; }
}
