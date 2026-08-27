using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Repositories.Interfaces;

public interface IAdminIdentityRepository : IRepository
{
	Task<List<AdminIdentity>> GetAdminIdentitiesAsync(IEnumerable<string> ids);

	Task<AdminIdentity> GetAdminIdentityAsync(string id);

	void AddAdminIdentity(AdminIdentity identity);

	void DeleteAdminIdentity(AdminIdentity identity);
}
