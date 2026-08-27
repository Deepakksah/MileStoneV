using System;
using System.Globalization;
using VideoOS.Utilities.Database;

namespace VideoOS.Common.Integration;

public class SqlDataFilter : SqlLogResourceComponentBase, ILogAuditLogger
{
	private ILogAuditLogger _dataAccessor;

	private bool? _userAccessLogging;

	public SqlDataFilter(ISqlConnectionManager sqlConnectionManager, ILogAuditLogger dataAccessor)
		: base(sqlConnectionManager, 0, 0)
	{
		_dataAccessor = dataAccessor;
	}

	public void AddAuditLogEntry(LogEntryAudit logEntryAudit)
	{
		if (logEntryAudit == null)
		{
			throw new ArgumentNullException("logEntryAudit");
		}
		if (LogEntryDefinitions.IsExtendedLog((LogEntryId)logEntryAudit.LogId))
		{
			if (!_userAccessLogging.HasValue)
			{
				PrepareSqlBatch();
				AddStatement("SELECT [Setting] FROM [SystemSettings] WHERE [Name] = 'LogAuditEnableUserAccessLogging'");
				string value = (string)ExecuteScalar();
				try
				{
					_userAccessLogging = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
				}
				catch
				{
					return;
				}
			}
			if (!_userAccessLogging.Value)
			{
				return;
			}
		}
		_dataAccessor.AddAuditLogEntry(logEntryAudit);
	}
}
