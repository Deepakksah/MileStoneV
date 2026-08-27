using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Repositories.Interfaces;

public interface ITrustedIssuerRepository : IRepository
{
	Task<IEnumerable<TrustedIssuer>> GetAsync();

	Task<TrustedIssuer> GetByIdAsync(int id);

	void Add(TrustedIssuer issuer);

	void Delete(TrustedIssuer issuer);

	Task<TrustedIssuer> GetByIssuerAsync(string issuer);

	Task<bool> IssuerExistsAsync(string issuer);

	Task<bool> AddressExistsAsync(Uri address);
}
