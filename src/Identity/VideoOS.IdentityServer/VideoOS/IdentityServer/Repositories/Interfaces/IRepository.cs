using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Repositories.Interfaces;

public interface IRepository
{
	Task SaveChangesAsync();
}
