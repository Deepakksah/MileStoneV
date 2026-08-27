using System.Data.Common;
using Microsoft.Data.SqlClient;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public sealed class IdpRegistrySqlConnectionStorage : BaseSqlConnectionReadonlyStorage
{
	private const string XpcoRegistryPath = "SOFTWARE\\VideoOS\\Server\\Common";

	private const string XpcoRegistryKeyName = "Connectionstring";

	private const string IdpInitialCatalogAppendage = "_IDP";

	public IdpRegistrySqlConnectionStorage()
		: base("SOFTWARE\\VideoOS\\Server\\Common", "Connectionstring")
	{
	}

	public IdpRegistrySqlConnectionStorage(IRegistryService registryService)
		: base("SOFTWARE\\VideoOS\\Server\\Common", "Connectionstring", registryService)
	{
	}

	public override string RetrieveConnectionString()
	{
		string defaultConnectionString = base.RetrieveConnectionString();
		return UpdateServerAndDatabase(defaultConnectionString);
	}

	private string UpdateServerAndDatabase(string defaultConnectionString)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (string.IsNullOrEmpty(defaultConnectionString))
		{
			return string.Empty;
		}
		SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(defaultConnectionString);
		val.InitialCatalog += "_IDP";
		return ((DbConnectionStringBuilder)val).ConnectionString;
	}
}
