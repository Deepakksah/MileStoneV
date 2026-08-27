using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Services.CleanupInactiveUsers;

public interface IInactiveUserCleanupService
{
	Task CleanupInactiveUsersAsync();
}
