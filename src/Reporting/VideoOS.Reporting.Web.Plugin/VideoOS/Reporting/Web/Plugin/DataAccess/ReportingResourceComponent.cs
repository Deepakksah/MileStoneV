using System;
using VideoOS.Utilities.Database;

namespace VideoOS.Reporting.Web.Plugin.DataAccess;

public class ReportingResourceComponent : SqlResourceComponent
{
	protected override bool IsDebugEnabled => false;

	public ReportingResourceComponent()
		: base(SqlConnectionManagerSingleton.Manager, 0, 0)
	{
	}

	protected override void MapException(DatabaseException ex)
	{
		throw ex;
	}

	protected override void AddLogEntryDatabaseConnectionLost()
	{
	}

	protected override void LogDebug(string entry)
	{
	}

	protected override void LogWarning(string entryFormat, params object[] args)
	{
	}

	protected override void Trace(Exception ex)
	{
	}

	protected override void Trace(string entry)
	{
	}
}
