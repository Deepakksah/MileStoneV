using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Repositories;

public class LoginProviderRepository : ILoginProviderRepository, IRepository
{
	private readonly IApplicationDbContext _dbContext;

	public LoginProviderRepository(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void CreateLoginProvider(LoginProvider loginProvider)
	{
		_dbContext.LoginProviders.Add(loginProvider);
	}

	public void UpdateLoginProvider(LoginProvider loginProvider)
	{
		_dbContext.LoginProviders.Update(loginProvider);
	}

	public void DeleteLoginProvider(LoginProvider loginProvider)
	{
		_dbContext.LoginProviders.Remove(loginProvider);
	}

	public async Task<LoginProvider> GetLoginProvider(Guid providerId)
	{
		return await _dbContext.LoginProviders.Include((LoginProvider f) => f.ScimClients).FirstOrDefaultAsync((LoginProvider t) => t.Id.Equals(providerId));
	}

	public async Task<IEnumerable<LoginProvider>> GetLoginProviders()
	{
		return await _dbContext.LoginProviders.Include((LoginProvider f) => f.ScimClients).ToListAsync();
	}

	public async Task<bool> HasAny()
	{
		return await _dbContext.LoginProviders.AnyAsync();
	}

	public async Task<bool> HasAny(Guid id)
	{
		return await _dbContext.LoginProviders.AnyAsync((LoginProvider p) => p.Id == id);
	}

	public async Task<bool> HasAnyWithDisplayName(string displayName)
	{
		return await _dbContext.LoginProviders.AnyAsync((LoginProvider p) => p.DisplayName == displayName);
	}

	public async Task<bool> HasAnyWithAuthority(string authority)
	{
		return await _dbContext.LoginProviders.AnyAsync((LoginProvider p) => p.Authority == authority);
	}

	public async Task SaveChangesAsync()
	{
		if (_dbContext is ApplicationDbContext applicationDbContext)
		{
			await applicationDbContext.SaveChangesAsync();
			return;
		}
		throw new NotSupportedException("SaveChangesAsync is only supported for ApplicationDbContext");
	}

	public async Task<IEnumerable<LoginProvider>> GetClientsMappedProviders(string[] clientIds)
	{
		if (clientIds == null || clientIds.Length == 0)
		{
			return Enumerable.Empty<LoginProvider>();
		}
		List<LoginProvider> source = (await _dbContext.LoginProviders.Include((LoginProvider f) => f.ScimClients).ToListAsync()) ?? new List<LoginProvider>();
		return source.Where((LoginProvider p) => p.ScimClients.Select((LoginProviderClient f) => f.ClientId).Intersect(clientIds).Any()).ToList();
	}
}
