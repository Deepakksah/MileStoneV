using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Repository.EntityFramework.Setup;

public sealed class InitializeRepository : IInitializeRepository
{
	public void Initialize([NotNull] string connectionString)
	{
		connectionString.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Repository.EntityFramework\\Setup\\InitializeRepository.cs", 13);
		new IncidentManagerContext(connectionString).InitializeDatabase();
	}
}
