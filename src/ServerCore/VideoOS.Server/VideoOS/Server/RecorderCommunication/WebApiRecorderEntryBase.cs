using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.WebApi;

namespace VideoOS.Server.RecorderCommunication;

public abstract class WebApiRecorderEntryBase : RecorderEntryBase
{
	private EventTracer _eventTracer;

	public static WebApiRecorderEntryBase CreateRecorderEntry(Guid recorderId, Uri recorderAddress, Guid serviceId, RecordingServerServiceType serviceType, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus, RecorderVersion version)
	{
		return version switch
		{
			RecorderVersion.Version253 => new RecorderEntry253WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version252 => new RecorderEntry252WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version251 => new RecorderEntry251WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version242 => new RecorderEntry242WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version241 => new RecorderEntry241WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version233 => new RecorderEntry233WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version232 => new RecorderEntry232WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version231 => new RecorderEntry231WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version223 => new RecorderEntry223WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version222 => new RecorderEntry222WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version221 => new RecorderEntry221WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version212 => new RecorderEntry212WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version211 => new RecorderEntry211WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version203 => new RecorderEntry203WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version202 => new RecorderEntry202WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version201 => new RecorderEntry201WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			RecorderVersion.Version132 => new RecorderEntry132WebApi(recorderId, recorderAddress, serviceId, serviceType, connectionStatus, attachStatus, recorderStatus), 
			_ => throw new ArgumentException("Recorder communication object is null or not supported"), 
		};
	}

	public WebApiRecorderEntryBase(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, Uri recorderUri, RecorderConnectionStatus connectionStatus, RecorderAttachStatus attachStatus, RecorderStatuses recorderStatus)
		: base(recorderId, serviceId, serviceType, recorderUri.ToString(), connectionStatus, attachStatus, recorderStatus)
	{
		_eventTracer = new EventTracer();
		base.ServiceConnectionStatus = new ServiceConnectionStatus(serviceId, ServerConfig.Instance.WebApiConfig.ServiceDiscoveryTimeout, this);
		_sendCommandSender = new BulkMessageProcessorAsync<CommandPackage>(ProcessCommandAsync, _eventTracer, 100);
		_sendResponseSender = new MessageProcessorAsync<ResponseBase>(ProcessResponseAsync);
		_sendCommandSender.Start(enable: true);
		_sendResponseSender.Start(enable: true);
	}

	protected abstract Task SendCommandToRecorderAsync(CommandBase command, CommunicationCallContextDto callContextInfo);

	public async Task ProcessCommandAsync(CommandPackage commandPackage)
	{
		try
		{
			ProcessMessageBase(commandPackage);
			CommunicationCallContextDto callContextInfo = new CommunicationCallContextDto(commandPackage.CallContextInfo.UserSid, commandPackage.CallContextInfo.UserDomainAccountName, commandPackage.CallContextInfo.LocationIPAddress);
			await SendCommandToRecorderAsync(commandPackage.CommandBase, callContextInfo);
			RecorderCommManager.Logger.LogDebug("Command sent. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", base.RecorderId, commandPackage.CommandBase.GetType().Name, commandPackage.CommandBase.Token);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			RecorderCommManager.Logger.LogError("Failed to send command. Recorder: {RecorderId}; Type: {Type}; Exception: {Exception}", base.RecorderId, commandPackage.CommandBase.GetType().Name, ex2);
		}
	}

	protected abstract Task SendResponseToRecorderAsync(ResponseBase response);

	private async Task ProcessResponseAsync(ResponseBase response)
	{
		try
		{
			RecorderCommManager.Logger.LogInformation("Sending response. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", base.RecorderId, response.GetType().Name, response.Token);
			await SendResponseToRecorderAsync(response);
			RecorderCommManager.Logger.LogDebug("Response sent. Recorder: {RecorderId}; Type: {Type}; Token: {Token}", base.RecorderId, response.GetType().Name, response.Token);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			RecorderCommManager.Logger.LogError("Failed to send response to recorder. Recorder: {RecorderId}; Type: {Type}; Exception: {Exception}", base.RecorderId, response.GetType().Name, ex2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (disposing && _eventTracer != null)
		{
			_eventTracer.Dispose();
		}
	}
}
