#define TRACE
using System;
using System.Diagnostics;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Utilities;

public class ServerAdminResourceComponent : SqlResourceComponent
{
	protected override bool IsDebugEnabled => false;

	protected ServerAdminResourceComponent(ISqlConnectionManager sqlConnectionManager)
		: base(sqlConnectionManager, 2000, 20)
	{
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

	protected override void MapException(DatabaseException ex)
	{
		throw ex;
	}

	protected override void Trace(Exception ex)
	{
		System.Diagnostics.Trace.WriteLine(ex);
	}

	protected override void Trace(string entry)
	{
		System.Diagnostics.Trace.WriteLine(entry);
	}
}
