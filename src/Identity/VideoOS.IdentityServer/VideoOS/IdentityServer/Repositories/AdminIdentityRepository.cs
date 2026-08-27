using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Repositories;

public class AdminIdentityRepository : IAdminIdentityRepository, IRepository
{
	private readonly ApplicationDbContext _dbContext;

	public AdminIdentityRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public void AddAdminIdentity(AdminIdentity identity)
	{
		_dbContext.AdminIdentities.Add(identity);
	}

	public async Task<List<AdminIdentity>> GetAdminIdentitiesAsync(IEnumerable<string> ids)
	{
		return await (from adminIdentity in _dbContext.AdminIdentities
			where ids.Contains(adminIdentity.Identity)
			select (adminIdentity)).ToListAsync();
	}

	public async Task<AdminIdentity> GetAdminIdentityAsync(string id)
	{
		return await _dbContext.AdminIdentities.FirstOrDefaultAsync((AdminIdentity t) => t.Identity.Equals(id));
	}

	public void DeleteAdminIdentity(AdminIdentity identity)
	{
		_dbContext.AdminIdentities.Remove(identity);
	}

	public async Task SaveChangesAsync()
	{
		await _dbContext.SaveChangesAsync();
	}
}
