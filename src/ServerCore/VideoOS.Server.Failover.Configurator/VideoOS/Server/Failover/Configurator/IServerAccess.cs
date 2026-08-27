using VideoOS.Management.VmoClient;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server.Failover.Configurator;

public interface IServerAccess
{
	ServerConnection CreateConnection();

	VmoClient CreateVmoClient(ServerConnection connection);

	CorporateSqlData CreateCorporateSqlAccessor();
}
