using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace VideoOS.SqlUpdate.Core.Connection;

public class ConnectionStringHelper
{
	public static string GetConnectionStringToMasterDb(string connectionString)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		return ((DbConnectionStringBuilder)new SqlConnectionStringBuilder(connectionString)
		{
			InitialCatalog = "master"
		}).ConnectionString;
	}

	public static string ExtractDbName(string connectionString)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
	}
}
