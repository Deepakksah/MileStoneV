using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;

namespace VideoOS.IdentityServer.Repositories.Interfaces;

public interface IConfigurationRepository : IRepository
{
	Task AddAsync(Client client);

	Task<Client> GetByClientIdAsync(string clientId, params Expression<Func<Client, object>>[] includes);

	Task<IEnumerable<Client>> GetClientsAsync(params Expression<Func<Client, object>>[] includes);

	Task<IEnumerable<string>> GetAllValidScopesAsync();
}
