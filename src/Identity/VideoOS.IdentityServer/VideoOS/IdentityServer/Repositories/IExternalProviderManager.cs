using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Repositories;

public interface IExternalProviderManager
{
	void AddProvider(LoginProvider loginProvider);

	void UpdateProvider(LoginProvider loginProvider);

	void RemoveProvider(string name, string displayName);
}
