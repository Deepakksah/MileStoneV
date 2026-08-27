using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Repositories.Interfaces;

public interface ILoginProviderRepository : IRepository
{
	Task<LoginProvider> GetLoginProvider(Guid providerId);

	Task<IEnumerable<LoginProvider>> GetLoginProviders();

	void CreateLoginProvider(LoginProvider loginProvider);

	void UpdateLoginProvider(LoginProvider loginProvider);

	void DeleteLoginProvider(LoginProvider loginProvider);

	Task<bool> HasAny();

	Task<bool> HasAny(Guid id);

	Task<bool> HasAnyWithDisplayName(string displayName);

	Task<bool> HasAnyWithAuthority(string authority);

	Task<IEnumerable<LoginProvider>> GetClientsMappedProviders(string[] clientIds);
}
