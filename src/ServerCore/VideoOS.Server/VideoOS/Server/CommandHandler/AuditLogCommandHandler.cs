using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Logging;
using VideoOS.Common.Integration.Properties;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.CommandHandler;

public class AuditLogCommandHandler : ICommandHandler
{
	private readonly IServerEnvironment _server;

	public AuditLogCommandHandler(IServerEnvironment server)
	{
		_server = server;
	}

	private static string GetAuditLogDescription(LogEntryId logEntryId)
	{
		return logEntryId switch
		{
			LogEntryId.AuditConfigReportExported => LogDefinitions.AuditConfigReportExported, 
			LogEntryId.AuditPtzReserveDenied => LogDefinitions.AuditPtzReserveDenied, 
			LogEntryId.AuditPtzReserveGranted => LogDefinitions.AuditPtzReserveGranted, 
			LogEntryId.AuditGenericEventsActivated => LogDefinitions.AuditGenericEventsActivated, 
			LogEntryId.AuditGenericEventsDeactivated => LogDefinitions.AuditGenericEventsDeactivated, 
			_ => null, 
		};
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is AuditLogCommand auditLogCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			auditLogCommand.AuditLog.Location = callContextInfo.LocationIPAddress;
			auditLogCommand.AuditLog.UserCredentials = callContextInfo.UserDomainAccountName;
			ServerInfoCollector serverInfoCollector = new ServerInfoCollector();
			auditLogCommand.AuditLog.ResourceLocation = serverInfoCollector.ManagementServerName();
			if (string.Compare(auditLogCommand.AuditLog.ResourceTypeValue.ToString(), ResourceType.ManagementServer.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
			{
				auditLogCommand.AuditLog.ResourceName = serverInfoCollector.ManagementServerName();
			}
			ServerLogManager.Instance.ServerLogEntryAuditLogger.LogEntryAuditLog(auditLogCommand.AuditLog);
			LogEntryId logEntryId = (LogEntryId)auditLogCommand.AuditLog.LogId;
			if (GetAuditLogDescription(logEntryId) != null)
			{
				Guid resourceId = Guid.Parse(auditLogCommand.AuditLog.ResourceID);
				ResourceType resourceType = (ResourceType)Enum.Parse(typeof(ResourceType), auditLogCommand.AuditLog.ResourceTypeValue);
				AuditType auditType = (AuditType)Enum.Parse(typeof(AuditType), auditLogCommand.AuditLog.AuditTypeValue);
				AccessGranted accessGranted = (AccessGranted)Enum.Parse(typeof(AccessGranted), auditLogCommand.AuditLog.AccessGrantedValue);
				SourceType sourceType = (SourceType)Enum.Parse(typeof(SourceType), auditLogCommand.AuditLog.SourceTypeValue);
				ILocalizedLogger auditAndSystemLogger = _server.AuditAndSystemLogger;
				LogEntryId logEntryId2 = logEntryId;
				string resourceName = auditLogCommand.AuditLog.ResourceName;
				string resourceLocation = auditLogCommand.AuditLog.ResourceLocation;
				string locationIPAddress = callContextInfo.LocationIPAddress;
				string userCredentials = auditLogCommand.AuditLog.UserCredentials;
				Func<string> description = () => GetAuditLogDescription(logEntryId);
				object[] arguments = auditLogCommand.AuditLog.Arguments;
				auditAndSystemLogger.Log(logEntryId2, resourceId, resourceName, resourceLocation, resourceType, sourceType, locationIPAddress, userCredentials, auditType, accessGranted, description, arguments);
			}
			responseSink.AddResponse(new SuccessResponse());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogWarning("Failed to execute Audit Log command. Error: {ExceptionMessage}", ex2.Message);
			responseSink.AddResponse(new FailResponse());
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
