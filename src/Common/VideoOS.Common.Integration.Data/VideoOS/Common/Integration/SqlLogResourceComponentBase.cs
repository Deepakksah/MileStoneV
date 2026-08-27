using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration;

public abstract class SqlLogResourceComponentBase : SqlResourceComponent
{
	private static readonly ILogger Logger = Telemetry.GetILogger("SQLLog");

	protected override bool IsDebugEnabled => Logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug);

	protected SqlLogResourceComponentBase(ISqlConnectionManager sqlConnectionManager, int deadlockPause, int deadlockRetries)
		: base(sqlConnectionManager, deadlockPause, deadlockRetries)
	{
	}

	protected override void AddLogEntryDatabaseConnectionLost()
	{
		Logger.LogDebug("Database connection lost");
	}

	protected override void LogDebug(string entry)
	{
		Logger.LogDebug(entry);
	}

	protected override void LogWarning(string entryFormat, params object[] args)
	{
		Logger.LogWarning(entryFormat, args);
	}

	protected override void MapException(DatabaseException ex)
	{
		throw ex;
	}

	protected override void Trace(string entry)
	{
		Logger.LogDebug(entry);
	}

	protected override void Trace(Exception ex)
	{
		if (ex != null)
		{
			Logger.LogDebug(ex.ToString());
		}
	}
}
