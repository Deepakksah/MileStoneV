using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Properties;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration.Logging;

public static class AuditAndSystemLoggingExtensions
{
	public static void Log(this ILocalizedLogger localizedLogger, LogEntryId logEntryId, Guid resourceId, string resourceName, string resourceLocation, ResourceType resourceType, SourceType sourceType, string locationIPAddress, string UserName, AuditType auditType, AccessGranted accessGranted, Func<string> description, params object[] descriptionArgs)
	{
		localizedLogger.Log((LogLevel)(LogEntryDefinitions.IsExtendedLog(logEntryId) ? 1 : 2), (CultureInfo c) => new LogItem[12]
		{
			new LogItem(LogDefinitions.Field_LogType, ServerLogType.Audit, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_LogEntryId, logEntryId.ToString()),
			new LogItem(LogDefinitions.Field_ResourceId, resourceId),
			new LogItem(LogDefinitions.Field_ResourceName, resourceName),
			new LogItem(LogDefinitions.Field_ResourceLocation, resourceLocation),
			new LogItem(LogDefinitions.Field_ResourceType, resourceType, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_Description, description(), descriptionArgs),
			new LogItem(LogDefinitions.Field_SourceType, sourceType, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_LocationIPAddress, locationIPAddress),
			new LogItem(LogDefinitions.Field_UserName, UserName),
			new LogItem(LogDefinitions.Field_AuditType, auditType, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_AccessGranted, accessGranted, LogDefinitions.ResourceManager)
		});
	}

	public static void Log(this ILocalizedLogger localizedLogger, LogEntryId logEntryId, string resourceId, string resourceName, string resourceLocation, ResourceType resourceType, SourceType sourceType, string locationIPAddress, string userName, AuditType auditType, AccessGranted accessGranted, Func<string> description, params object[] descriptionArgs)
	{
		localizedLogger.Log((LogLevel)(LogEntryDefinitions.IsExtendedLog(logEntryId) ? 1 : 2), (CultureInfo c) => new LogItem[12]
		{
			new LogItem(LogDefinitions.Field_LogType, ServerLogType.Audit, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_LogEntryId, logEntryId.ToString()),
			new LogItem(LogDefinitions.Field_ResourceId, resourceId),
			new LogItem(LogDefinitions.Field_ResourceName, resourceName),
			new LogItem(LogDefinitions.Field_ResourceLocation, resourceLocation),
			new LogItem(LogDefinitions.Field_ResourceType, resourceType, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_Description, description(), descriptionArgs),
			new LogItem(LogDefinitions.Field_SourceType, sourceType, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_LocationIPAddress, locationIPAddress),
			new LogItem(LogDefinitions.Field_UserName, userName),
			new LogItem(LogDefinitions.Field_AuditType, auditType, LogDefinitions.ResourceManager),
			new LogItem(LogDefinitions.Field_AccessGranted, accessGranted, LogDefinitions.ResourceManager)
		});
	}
}
