using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class PingDeviceCommandHandler : ICommandHandler
{
	private readonly IServerEnvironment _serverEnvironment;

	public PingDeviceCommandHandler(IServerEnvironment server)
	{
		_serverEnvironment = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is PingDeviceCommand pingDeviceCommand))
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
		if (pingDeviceCommand.RecorderId == Guid.Empty)
		{
			responseSink.AddResponse(new FailResponse());
			return;
		}
		RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(pingDeviceCommand.RecorderId);
		if (recordingServer == null)
		{
			Server.Instance.Logger.LogWarning("Could not find recorder with id = {RecorderId}.", pingDeviceCommand.RecorderId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		if (recordingServer.Type != RecordingServerServiceType.RecordingServer)
		{
			Server.Instance.Logger.LogWarning("Not possible to run database command on failover server. RecorderId = {RecorderId}.", pingDeviceCommand.RecorderId);
			responseSink.AddResponse(new RecorderUnavailableResponse(pingDeviceCommand.RecorderId));
			return;
		}
		if (!_serverEnvironment.RecorderCommManager.IsRecorderAttachedAndConnected(recordingServer.ServiceId))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(pingDeviceCommand.RecorderId));
			return;
		}
		try
		{
			IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(recordingServer.ServiceId, pingDeviceCommand, callContextInfo);
			ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(pingDeviceCommand.Timeout.Add(TimeSpan.FromSeconds(1.0)));
			responseSink.AddResponse(nextResponse);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(pingDeviceCommand.RecorderId));
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(pingDeviceCommand.RecorderId));
		}
	}
}
