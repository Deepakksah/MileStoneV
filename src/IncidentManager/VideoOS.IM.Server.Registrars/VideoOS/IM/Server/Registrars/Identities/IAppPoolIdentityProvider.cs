using VideoOS.IM.Server.Registrars.Properties;

namespace VideoOS.IM.Server.Registrars.Identities;

public interface IAppPoolIdentityProvider
{
	[NotNull]
	string GetAppPoolIdentity([NotNull] string appPoolName);
}
