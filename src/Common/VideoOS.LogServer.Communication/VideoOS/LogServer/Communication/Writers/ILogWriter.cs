using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.LogServer.Communication.LogWriterServiceReference;

namespace VideoOS.LogServer.Communication.Writers;

public interface ILogWriter : IDisposable
{
	Task RegisterSourceTypesAsync(IEnumerable<LogMessageSourceType> sourceTypes);

	Task RegisterSourceTypesTokenAsync(string token, IEnumerable<LogMessageSourceType> sourceTypes);

	Task RegisterCategoriesAsync(IEnumerable<LogMessageCategory> categories);

	Task RegisterCategoriesTokenAsync(string token, IEnumerable<LogMessageCategory> categories);

	Task RegisterLogMessagesAsync(IEnumerable<LogMessage> logMessages);

	Task RegisterLogMessagesTokenAsync(string token, IEnumerable<LogMessage> logMessages);

	Task RegisterLegacyLogMessagesAsync(IEnumerable<LegacyLogMessage> logMessages);

	Task RegisterLegacyLogMessagesTokenAsync(string token, IEnumerable<LegacyLogMessage> logMessages);

	Task WriteLoglineAsync(Guid messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType);

	Task WriteLoglineTokenAsync(string token, Guid messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType);

	Task WriteLegacyLoglineAsync(string messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType);

	Task WriteLegacyLoglineTokenAsync(string token, string messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType);
}
