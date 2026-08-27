using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Repositories;

public class TrustedIssuerRepository : ITrustedIssuerRepository, IRepository
{
	private readonly ApplicationDbContext _dbContext;

	public TrustedIssuerRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<TrustedIssuer> GetByIdAsync(int id)
	{
		return await _dbContext.TrustedIssuers.FirstOrDefaultAsync((TrustedIssuer p) => p.Id == id);
	}

	public async Task<IEnumerable<TrustedIssuer>> GetAsync()
	{
		return await _dbContext.TrustedIssuers.ToListAsync();
	}

	public async Task<TrustedIssuer> GetByIssuerAsync(string issuer)
	{
		return await _dbContext.TrustedIssuers.FirstOrDefaultAsync((TrustedIssuer p) => p.Issuer == issuer);
	}

	public void Add(TrustedIssuer issuer)
	{
		_dbContext.TrustedIssuers.Add(issuer);
	}

	public void Delete(TrustedIssuer issuer)
	{
		_dbContext.TrustedIssuers.Remove(issuer);
	}

	public async Task SaveChangesAsync()
	{
		await _dbContext.SaveChangesAsync();
	}

	public async Task<bool> IssuerExistsAsync(string issuer)
	{
		return await _dbContext.TrustedIssuers.AnyAsync((TrustedIssuer p) => p.Issuer == issuer);
	}

	public async Task<bool> AddressExistsAsync(Uri address)
	{
		return await _dbContext.TrustedIssuers.AnyAsync((TrustedIssuer p) => p.Address == address);
	}
}
