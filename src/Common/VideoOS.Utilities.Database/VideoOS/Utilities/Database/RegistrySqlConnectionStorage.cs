using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public class RegistrySqlConnectionStorage : BaseSqlConnectionReadonlyStorage
{
	private const string XpcoRegistryPath = "SOFTWARE\\VideoOS\\Server\\Common";

	private const string XpcoRegistryKeyName = "Connectionstring";

	public RegistrySqlConnectionStorage()
		: base("SOFTWARE\\VideoOS\\Server\\Common", "Connectionstring")
	{
	}

	public RegistrySqlConnectionStorage(IRegistryService registryService)
		: base("SOFTWARE\\VideoOS\\Server\\Common", "Connectionstring", registryService)
	{
	}
}
