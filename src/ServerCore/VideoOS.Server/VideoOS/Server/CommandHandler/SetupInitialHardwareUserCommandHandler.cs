using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class SetupInitialHardwareUserCommandHandler : ICommandHandler
{
	private readonly IServerEnvironment _serverEnvironment;

	public SetupInitialHardwareUserCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is SetupInitialHardwareUserCommand setupInitialHardwareUserCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		if (setupInitialHardwareUserCommand.RecorderId == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("(SetupInitialHardwareUserCommand) Empty Recorder Id sent");
			responseSink.AddResponse(new FailResponse());
			return;
		}
		Guid guid = _serverEnvironment.RecorderCommManager.QueryRecorderServiceId(setupInitialHardwareUserCommand.RecorderId);
		if (guid == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("(SetupInitialHardwareUserCommand) Could not find recording service for recorder with id = {RecorderId}.", setupInitialHardwareUserCommand.RecorderId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		if (!_serverEnvironment.RecorderCommManager.IsRecorderAttachedAndConnected(guid))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(guid));
			return;
		}
		try
		{
			CreateAuditLogEntry(setupInitialHardwareUserCommand, callContextInfo);
			IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(guid, setupInitialHardwareUserCommand, callContextInfo);
			ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(90.0));
			responseSink.AddResponse(nextResponse);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(guid));
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(guid));
		}
	}

	private void CreateAuditLogEntry(SetupInitialHardwareUserCommand cmd, ICallContextInfo callContextInfo)
	{
		string userDomainAccountName = callContextInfo.UserDomainAccountName;
		string locationIPAddress = callContextInfo.LocationIPAddress;
		string recordingServerName = FindRecorderNameByRecorderId(cmd.RecorderId);
		_serverEnvironment.ServerLogManager.AuditLogger.AuditHardwarePreConfigurationStarted(cmd.HardwareUri.ToString(), cmd.HardwareUri.Host.ToString(), recordingServerName, userDomainAccountName, locationIPAddress);
	}

	private string FindRecorderNameByRecorderId(Guid recorderID)
	{
		string result = string.Empty;
		try
		{
			result = _serverEnvironment.ServerConfigurationLookup.FindRecorderDisplayName(recorderID);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
		return result;
	}
}
