using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class SetHardwareNetworkSettingsCommandHandler : ICommandHandler
{
	private class MessageManagerCallback : IMessageManagerCallback, IDisposable
	{
		private readonly IResponseSink _responseSink;

		private readonly ManualResetEvent _completeEvent;

		public MessageManagerCallback(IResponseSink responseSink)
		{
			_responseSink = responseSink;
			_completeEvent = new ManualResetEvent(initialState: false);
		}

		public void WaitForCompletion()
		{
			_completeEvent.WaitOne();
		}

		void IMessageManagerCallback.HandleTimeout(CommandBase command)
		{
			_responseSink.AddResponse(new TimeoutResponse());
			_completeEvent.Set();
		}

		void IMessageManagerCallback.HandleResponse(CommandBase command, ResponseBase response)
		{
			_responseSink.AddResponse(response);
		}

		bool IMessageManagerCallback.IsLastExpectedResponse(CommandBase command, ResponseBase response)
		{
			_completeEvent.Set();
			return true;
		}

		public void Dispose()
		{
			_completeEvent.Close();
		}
	}

	private readonly IServerEnvironment _serverEnvironment;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	public SetHardwareNetworkSettingsCommandHandler(IServerEnvironment serverEnvironment)
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
		if (!(command is SetHardwareNetworkSettingsCommand setHardwareNetworkSettingsCommand))
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
		if (setHardwareNetworkSettingsCommand.HardwareId == Guid.Empty)
		{
			responseSink.AddResponse(new FailResponse());
			return;
		}
		Guid recordingServerId = GetRecordingServerId(setHardwareNetworkSettingsCommand.HardwareId);
		if (recordingServerId == Guid.Empty)
		{
			Logger.LogWarning("Could not find recorder id for hardware = {HardwareId}.", setHardwareNetworkSettingsCommand.HardwareId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		Guid guid = QueryActiveRecordingServer(recordingServerId);
		if (guid == Guid.Empty)
		{
			Logger.LogWarning("Could not find recorder with id = {ServerId}.", recordingServerId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		if (!_serverEnvironment.RecorderCommManager.IsRecorderAttachedAndConnected(guid))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(guid));
			return;
		}
		using MessageManagerCallback messageManagerCallback = new MessageManagerCallback(responseSink);
		try
		{
			_serverEnvironment.MessageManager.SendCommand(guid, setHardwareNetworkSettingsCommand, messageManagerCallback, TimeSpan.FromMinutes(1.0), callContextInfo);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(guid));
			return;
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(guid));
			return;
		}
		messageManagerCallback.WaitForCompletion();
	}

	private Guid GetRecordingServerId(Guid hardwareId)
	{
		using IHardwareConfigDataComponent hardwareConfigDataComponent = _serverEnvironment.DataComponentFactory.CreateHardwareConfigDataComponent();
		return hardwareConfigDataComponent.QueryHardwareById(hardwareId).RecorderId;
	}

	private Guid QueryActiveRecordingServer(Guid recorderId)
	{
		return _serverEnvironment.RecorderCommManager.QueryRecorderServiceId(recorderId);
	}
}
