using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.Configuration;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class HardwareSetupInformationCommandHandler : ICommandHandler
{
	private class MessageManagerCallback : IMessageManagerCallback, IDisposable
	{
		private readonly IResponseSink responseSink;

		private readonly ManualResetEvent completeEvent;

		private readonly IServerEnvironment serverEnvironment;

		public MessageManagerCallback(IServerEnvironment serverEnvironment, IResponseSink responseSink)
		{
			this.responseSink = responseSink;
			completeEvent = new ManualResetEvent(initialState: false);
			this.serverEnvironment = serverEnvironment;
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
			if (response is HardwareSetupInformationResponse hardwareSetupInformationResponse)
			{
				string definitionXml = hardwareSetupInformationResponse.HardwareSetupInformation.DefinitionXml;
				try
				{
					using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(serverEnvironment.ConfigurationDataLoader));
					EventTypes eventTypes = configurationManager.EventTypes;
					hardwareSetupInformationResponse.HardwareSetupInformation.DefinitionXml = DynamicEventUtility.CheckDynamicEventIds(serverEnvironment, eventTypes, definitionXml);
				}
				catch (Exception ex) when (ex is DatabaseException || ex is NullReferenceException)
				{
					responseSink.AddResponse(new ExceptionResponse(ex));
					return;
				}
			}
			responseSink.AddResponse(response);
		}

		bool IMessageManagerCallback.IsLastExpectedResponse(CommandBase command, ResponseBase response)
		{
			if (response is HardwareSetupInformationResponse)
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

	private readonly IServerEnvironment serverEnvironment;

	private readonly Server server;

	private IRecorderCommManager RecorderCommManager => serverEnvironment.RecorderCommManager;

	private ILicenseManager LicenseManager => serverEnvironment.LicenseManager;

	public HardwareSetupInformationCommandHandler(IServerEnvironment serverEnvironment, Server server)
	{
		this.serverEnvironment = serverEnvironment;
		this.server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is HardwareSetupInformationCommand hardwareSetupInformationCommand))
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
		bool aacSupported = LicenseManager.IsFeatureLicensed("AACAudioSupport");
		hardwareSetupInformationCommand.AacSupported = aacSupported;
		RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(hardwareSetupInformationCommand.RecorderId);
		if (recordingServer == null)
		{
			Server.Instance.Logger.LogWarning("Could not find recorder with id = {RecorderId}.", hardwareSetupInformationCommand.RecorderId);
			responseSink.AddResponse(new RecorderNotFoundResponse(hardwareSetupInformationCommand.RecorderId));
			return;
		}
		if (!RecorderCommManager.IsRecorderAttachedAndConnected(recordingServer.ServiceId))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(hardwareSetupInformationCommand.RecorderId));
			return;
		}
		using MessageManagerCallback messageManagerCallback = new MessageManagerCallback(serverEnvironment, responseSink);
		try
		{
			server.MessageManager.SendCommand(recordingServer.ServiceId, hardwareSetupInformationCommand, messageManagerCallback, TimeSpan.FromMinutes(2.0), callContextInfo);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(hardwareSetupInformationCommand.RecorderId));
			return;
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(hardwareSetupInformationCommand.RecorderId));
			return;
		}
		messageManagerCallback.WaitForCompletion();
	}
}
