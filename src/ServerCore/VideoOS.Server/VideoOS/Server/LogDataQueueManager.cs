using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class LogDataQueueManager : VmoResourceComponent
{
	public static class ReadLogAuditColumns
	{
		private static DatabaseColumnBinder PKID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder TimeStampUTC = new DatabaseColumnBinder("TimeStampUTC");

		private static DatabaseColumnBinder TimeStamp = new DatabaseColumnBinder("TimeStamp");

		private static DatabaseColumnBinder LogId = new DatabaseColumnBinder("LogID");

		private static DatabaseColumnBinder LogLevel = new DatabaseColumnBinder("LogLevel");

		private static DatabaseColumnBinder AccessGranted = new DatabaseColumnBinder("AccessGranted");

		private static DatabaseColumnBinder AuditType = new DatabaseColumnBinder("AuditType");

		private static DatabaseColumnBinder User = new DatabaseColumnBinder("User");

		private static DatabaseColumnBinder UserLocation = new DatabaseColumnBinder("UserLocation");

		private static DatabaseColumnBinder ResourceType = new DatabaseColumnBinder("ResourceType");

		private static DatabaseColumnBinder ResourceName = new DatabaseColumnBinder("ResourceName");

		private static DatabaseColumnBinder ResourceLocation = new DatabaseColumnBinder("ResourceLocation");

		private static DatabaseColumnBinder ResourceID = new DatabaseColumnBinder("ResourceID");

		private static DatabaseColumnBinder Argument0 = new DatabaseColumnBinder("Argument0");

		private static DatabaseColumnBinder Argument1 = new DatabaseColumnBinder("Argument1");

		private static DatabaseColumnBinder Argument2 = new DatabaseColumnBinder("Argument2");

		private static DatabaseColumnBinder Argument3 = new DatabaseColumnBinder("Argument3");

		private static DatabaseColumnBinder Argument4 = new DatabaseColumnBinder("Argument4");

		private static DatabaseColumnBinder Argument5 = new DatabaseColumnBinder("Argument5");

		private static DatabaseColumnBinder Argument6 = new DatabaseColumnBinder("Argument6");

		public static PoppedLogEntryAudit Bind(DatabaseDataReader reader)
		{
			int @int = LogId.GetInt32(reader);
			long int2 = PKID.GetInt64(reader);
			AccessGranted accessGranted = GetAccessGranted(AccessGranted.GetString(reader, allowNulls: false));
			AuditType auditType = GetAuditType(AuditType.GetString(reader, allowNulls: false));
			string userCredentials = User.GetString(reader, allowNulls: false);
			string location = UserLocation.GetString(reader, allowNulls: false);
			ResourceType resourceType = GetResourceType(ResourceType.GetString(reader, allowNulls: false));
			string resourceName = ResourceName.GetString(reader, allowNulls: false);
			string resourceLocation = ResourceLocation.GetString(reader, allowNulls: false);
			string text = ResourceID.GetString(reader, allowNulls: false);
			PoppedLogEntryAudit poppedLogEntryAudit = new PoppedLogEntryAudit((LogEntryId)@int, location, userCredentials, resourceType, resourceName, resourceLocation, text.Trim(), auditType, accessGranted, int2);
			poppedLogEntryAudit.TimeStampLocal = TimeStamp.GetDateTime(reader);
			poppedLogEntryAudit.TimeStampUtc = TimeStampUTC.GetDateTime(reader);
			poppedLogEntryAudit.LogLevel = GetLogLevel(LogLevel.GetString(reader, allowNulls: false));
			poppedLogEntryAudit.Arguments = new string[7]
			{
				Argument0.GetString(reader, allowNulls: true),
				Argument1.GetString(reader, allowNulls: true),
				Argument2.GetString(reader, allowNulls: true),
				Argument3.GetString(reader, allowNulls: true),
				Argument4.GetString(reader, allowNulls: true),
				Argument5.GetString(reader, allowNulls: true),
				Argument6.GetString(reader, allowNulls: true)
			};
			return poppedLogEntryAudit;
		}

		private static AccessGranted GetAccessGranted(string accessGranted)
		{
			if (Enum.GetNames(typeof(AccessGranted)).Contains(accessGranted, StringComparer.OrdinalIgnoreCase))
			{
				return (AccessGranted)Enum.Parse(typeof(AccessGranted), accessGranted, ignoreCase: true);
			}
			return VideoOS.Common.Integration.AccessGranted.Default;
		}

		private static AuditType GetAuditType(string auditType)
		{
			if (Enum.GetNames(typeof(AuditType)).Contains(auditType, StringComparer.OrdinalIgnoreCase))
			{
				return (AuditType)Enum.Parse(typeof(AuditType), auditType, ignoreCase: true);
			}
			return VideoOS.Common.Integration.AuditType.Unknown;
		}

		private static ResourceType GetResourceType(string resourceType)
		{
			if (Enum.GetNames(typeof(ResourceType)).Contains(resourceType, StringComparer.OrdinalIgnoreCase))
			{
				return (ResourceType)Enum.Parse(typeof(ResourceType), resourceType, ignoreCase: true);
			}
			return VideoOS.Common.Integration.ResourceType.Unknown;
		}

		private static LogLevel GetLogLevel(string logLevel)
		{
			if (Enum.GetNames(typeof(LogLevel)).Contains(logLevel, StringComparer.OrdinalIgnoreCase))
			{
				return (LogLevel)Enum.Parse(typeof(LogLevel), logLevel, ignoreCase: true);
			}
			return VideoOS.Common.Integration.LogLevel.Undefined;
		}
	}

	public class PoppedLogEntryAudit : LogEntryAudit
	{
		public long PKID { get; set; }

		public PoppedLogEntryAudit(LogEntryId logEntryId, string location, string userCredentials, ResourceType resourceType, string resourceName, string resourceLocation, string resourceId, AuditType auditType, AccessGranted accessGranted, long pkid)
			: base(logEntryId, location, SourceType.Unknown, null, userCredentials, resourceType, resourceName, resourceLocation, resourceId, auditType, accessGranted)
		{
			PKID = pkid;
			base.SourceId = Guid.Empty;
		}
	}

	private readonly int _numberOfLogsToRead;

	public LogDataQueueManager(int numberOfLogsToRead)
	{
		_numberOfLogsToRead = numberOfLogsToRead;
	}

	private List<PoppedLogEntryAudit> GetLogEntriesAuditFromQueue(int numberOfLogsToRead)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			List<PoppedLogEntryAudit> list = new List<PoppedLogEntryAudit>();
			PrepareSqlBatch();
			AddStatement("SELECT TOP (@NumberOfRows) [pkid], [TimeStamp], [TimeStampUTC], [LogID], [LogLevel], [AccessGranted], [AuditType], [User], [UserLocation], [ResourceType], [ResourceName], [ResourceLocation], [ResourceID], [Argument0], [Argument1], [Argument2], [Argument3], [Argument4], [Argument5], [Argument6] FROM [LogAuditVMOQueue] ORDER BY TimeStampUTC ASC");
			BindInt("@NumberOfRows", numberOfLogsToRead);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(ReadLogAuditColumns.Bind(databaseDataReader));
			}
			return list;
		});
	}

	public static string GetTableName(LogType logType)
	{
		if (logType == LogType.AuditLog)
		{
			return "[LogAuditVMOQueue]";
		}
		throw new ArgumentOutOfRangeException("logType");
	}

	private bool DeleteLogEntryFromDB(LogType logType, long pkid)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("delete from " + GetTableName(logType) + " where pkid = " + pkid.ToString(NumberFormatInfo.InvariantInfo));
			int num = Convert.ToInt32(ExecuteNonQuery(), NumberFormatInfo.InvariantInfo);
			return num == 1;
		});
	}

	public bool PopQueueAndSendToLogSvc(LogType logType)
	{
		switch (logType)
		{
		case LogType.AuditLog:
		{
			List<PoppedLogEntryAudit> logEntriesAuditFromQueue = GetLogEntriesAuditFromQueue(_numberOfLogsToRead);
			foreach (PoppedLogEntryAudit item in logEntriesAuditFromQueue)
			{
				LogDataAccessor.CreateLogEntry(item);
				DeleteLogEntryFromDB(logType, item.PKID);
			}
			return logEntriesAuditFromQueue.Count == _numberOfLogsToRead;
		}
		case LogType.SystemLog:
		case LogType.RuleLog:
			return false;
		default:
			throw new ArgumentOutOfRangeException("logType");
		}
	}
}
