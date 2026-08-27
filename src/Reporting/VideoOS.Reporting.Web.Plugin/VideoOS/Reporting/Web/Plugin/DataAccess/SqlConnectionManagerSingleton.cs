using System;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public static class SqlConnectionManagerSingleton
{
	private static readonly Lazy<SqlConnectionManager> _lazy = new Lazy<SqlConnectionManager>(() => Create());

	public static SqlConnectionManager Manager => _lazy.Value;

	private static SqlConnectionManager Create()
	{
		return new SqlConnectionManager(new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.ReportServer), null);
	}
}
