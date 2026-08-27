using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class PtzReleaseSessionCommandHandler : ICommandHandler
{
	private class MessageManagerCallback : IMessageManagerCallback, IDisposable
	{
		private IResponseSink responseSink;

		private ManualResetEvent completeEvent;

		public MessageManagerCallback(IResponseSink responseSink)
		{
			this.responseSink = responseSink;
			completeEvent = new ManualResetEvent(initialState: false);
		}

		public void WaitForCompletion()
		{
			completeEvent.WaitOne();
		}

		void IMessageManagerCallback.HandleTimeout(CommandBase command)
		{
			responseSink.AddResponse(new TimeoutResponse());
			completeEvent.Set();
		}

		void IMessageManagerCallback.HandleResponse(CommandBase command, ResponseBase response)
		{
			responseSink.AddResponse(response);
		}

		bool IMessageManagerCallback.IsLastExpectedResponse(CommandBase command, ResponseBase response)
		{
			if (response is ReleasePtzSessionResponse)
			{
				completeEvent.Set();
				return true;
			}
			return false;
		}

		public void Dispose()
		{
			completeEvent.Close();
		}
	}

	private Server server;

	public PtzReleaseSessionCommandHandler(Server server)
	{
		this.server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is PtzReleaseSessionCommand ptzReleaseSessionCommand))
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
		Guid empty = Guid.Empty;
		try
		{
			empty = ConfigurationDataManager.QueryRecorderIdByDeviceId(ptzReleaseSessionCommand.DeviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			string resourceString = ExceptionHandling.GetResourceString(ex2);
			if (string.IsNullOrEmpty(resourceString))
			{
				responseSink.AddResponse(new FailResponse());
			}
			else
			{
				responseSink.AddResponse(new ErrorResponse(1, resourceString));
			}
			return;
		}
		if (empty == Guid.Empty)
		{
			Server.Instance.Logger.LogWarning("Could not find recorder for device with id = {DeviceId}.", ptzReleaseSessionCommand.DeviceId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(empty);
		if (recordingServer == null)
		{
			Server.Instance.Logger.LogWarning("Could not find recorder with id = {RecorderId}.", empty);
			responseSink.AddResponse(new RecorderNotFoundResponse(empty));
			return;
		}
		if (!server.RecorderCommManager.IsRecorderAttachedAndConnected(recordingServer.ServiceId))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(empty));
			return;
		}
		using MessageManagerCallback messageManagerCallback = new MessageManagerCallback(responseSink);
		try
		{
			server.MessageManager.SendCommand(recordingServer.ServiceId, ptzReleaseSessionCommand, messageManagerCallback, TimeSpan.FromMinutes(2.0), callContextInfo);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(empty));
			return;
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(empty));
			return;
		}
		messageManagerCallback.WaitForCompletion();
	}
}
