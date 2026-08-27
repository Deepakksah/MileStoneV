using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class FirmwareUpgradeCommandHandler : ICommandHandler
{
	private readonly IServerEnvironment _serverEnvironment;

	public FirmwareUpgradeCommandHandler(IServerEnvironment serverEnvironment)
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
		if (!(command is FirmwareUpgradeCommand firmwareUpgradeCommand))
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
		if (firmwareUpgradeCommand.HardwareId == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("Could not find recorder id for hardware = {HardwareId}.", firmwareUpgradeCommand.HardwareId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		Guid recordingServerId = GetRecordingServerId(firmwareUpgradeCommand.HardwareId);
		if (recordingServerId == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("Could not find recorder id for hardware = {HardwareId}.", firmwareUpgradeCommand.HardwareId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		Guid guid = _serverEnvironment.RecorderCommManager.QueryRecorderServiceId(recordingServerId);
		if (guid == Guid.Empty)
		{
			_serverEnvironment.Server.Logger.LogWarning("Could not find recording service for recorder with id = {ServerId}.", recordingServerId);
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
			TimeSpan timeout = TimeSpan.FromSeconds(60.0);
			IMessageManagerResponseCollector messageManagerResponseCollector = _serverEnvironment.MessageManager.SendCommand(guid, firmwareUpgradeCommand, callContextInfo);
			ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(timeout);
			while (nextResponse is ProgressResponse)
			{
				responseSink.AddResponse(nextResponse);
				nextResponse = messageManagerResponseCollector.GetNextResponse(timeout);
			}
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

	private Guid GetRecordingServerId(Guid hardwareId)
	{
		using IHardwareConfigDataComponent hardwareConfigDataComponent = _serverEnvironment.DataComponentFactory.CreateHardwareConfigDataComponent();
		return hardwareConfigDataComponent.QueryHardwareById(hardwareId).RecorderId;
	}
}
