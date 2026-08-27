using System;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public class LogServerSqlConnectionStorage : BaseSqlConnectionReadonlyStorage
{
	private const string LogServerRegistryPathFormat = "SOFTWARE\\{0}\\{1}";

	private const string LogServerRegistryKeyName = "ConnectionString";

	public LogServerSqlConnectionStorage(ISqlRegistryPathResources sqlRegistryPathResources)
		: base(FormatRegistryPathWithOem(sqlRegistryPathResources), "ConnectionString")
	{
	}

	public LogServerSqlConnectionStorage(ISqlRegistryPathResources sqlRegistryPathResources, IRegistryService registryService)
		: base(FormatRegistryPathWithOem(sqlRegistryPathResources), "ConnectionString", registryService)
	{
	}

	private static string FormatRegistryPathWithOem(ISqlRegistryPathResources sqlRegistryPathResources)
	{
		if (sqlRegistryPathResources == null)
		{
			throw new ArgumentNullException("sqlRegistryPathResources");
		}
		return $"SOFTWARE\\{sqlRegistryPathResources.OemCompanyNameShort}\\{sqlRegistryPathResources.OemInstallDir}";
	}
}
