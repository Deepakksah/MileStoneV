using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface ILoginProviderIdpComponent : IDisposable
{
	LoginProviderData CreateLoginProvider(Guid id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes, string userNameClaimType, bool promptForLogin, bool enabled);

	LoginProviderData[] QueryLoginProviders();

	LoginProviderData QueryLoginProviderById(Guid id);

	LoginProviderData[] QueryLoginProvidersByIds(IEnumerable<Guid> ids);

	LoginProviderData UpdateLoginProvider(Guid id, string name, string clientId, string clientSecret, string clientSecretType, string callbackPath, string authority, IEnumerable<string> scopes, string userNameClaimType, bool? promptForLogin, bool? enabled);

	void DeleteLoginProvider(Guid id);
}
