using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Repositories;

public class ConfigurationRepository : IConfigurationRepository, IRepository
{
	private readonly IConfigurationDbContext _context;

	public ConfigurationRepository(IConfigurationDbContext context)
	{
		_context = context ?? throw new ArgumentNullException("context");
	}

	public async Task AddAsync(Client client)
	{
		await _context.Clients.AddAsync(client);
	}

	public async Task<Client> GetByClientIdAsync(string clientId, params Expression<Func<Client, object>>[] includes)
	{
		return await _context.Clients.IncludeMultiple(includes).FirstOrDefaultAsync((Client c) => c.ClientId == clientId).IgnoreContext();
	}

	public async Task<IEnumerable<Client>> GetClientsAsync(params Expression<Func<Client, object>>[] includes)
	{
		return await _context.Clients.IncludeMultiple(includes).ToListAsync();
	}

	public async Task<IEnumerable<string>> GetAllValidScopesAsync()
	{
		return (await _context.ApiResources.Include((ApiResource a) => a.Scopes).ToListAsync().IgnoreContext()).SelectMany((ApiResource a) => a.Scopes.Select((ApiResourceScope s) => s.Scope));
	}

	public Task SaveChangesAsync()
	{
		if (_context is ConfigurationDbContext configurationDbContext)
		{
			return configurationDbContext.SaveChangesAsync();
		}
		throw new NotSupportedException("SaveChangesAsync is only supported for ConfigurationDbContext");
	}
}
