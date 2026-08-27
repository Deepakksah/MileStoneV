using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RecorderStatusService2 : ClientBase<RecorderStatusService2Soap>, RecorderStatusService2Soap
{
	public RecorderStatusService2()
	{
	}

	public RecorderStatusService2(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public RecorderStatusService2(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public RecorderStatusService2(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public RecorderStatusService2(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetVersion()
	{
		return base.Channel.GetVersion();
	}

	public Task<int> GetVersionAsync()
	{
		return base.Channel.GetVersionAsync();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	StartStatusSessionResponse RecorderStatusService2Soap.StartStatusSession(StartStatusSessionRequest request)
	{
		return base.Channel.StartStatusSession(request);
	}

	public Guid StartStatusSession(string token)
	{
		StartStatusSessionRequest startStatusSessionRequest = new StartStatusSessionRequest();
		startStatusSessionRequest.Body = new StartStatusSessionRequestBody();
		startStatusSessionRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).StartStatusSession(startStatusSessionRequest).Body.StartStatusSessionResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<StartStatusSessionResponse> RecorderStatusService2Soap.StartStatusSessionAsync(StartStatusSessionRequest request)
	{
		return base.Channel.StartStatusSessionAsync(request);
	}

	public Task<StartStatusSessionResponse> StartStatusSessionAsync(string token)
	{
		StartStatusSessionRequest startStatusSessionRequest = new StartStatusSessionRequest();
		startStatusSessionRequest.Body = new StartStatusSessionRequestBody();
		startStatusSessionRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).StartStatusSessionAsync(startStatusSessionRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	StopStatusSessionResponse RecorderStatusService2Soap.StopStatusSession(StopStatusSessionRequest request)
	{
		return base.Channel.StopStatusSession(request);
	}

	public void StopStatusSession(string token, Guid statusSessionId)
	{
		StopStatusSessionRequest stopStatusSessionRequest = new StopStatusSessionRequest();
		stopStatusSessionRequest.Body = new StopStatusSessionRequestBody();
		stopStatusSessionRequest.Body.token = token;
		stopStatusSessionRequest.Body.statusSessionId = statusSessionId;
		((RecorderStatusService2Soap)this).StopStatusSession(stopStatusSessionRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<StopStatusSessionResponse> RecorderStatusService2Soap.StopStatusSessionAsync(StopStatusSessionRequest request)
	{
		return base.Channel.StopStatusSessionAsync(request);
	}

	public Task<StopStatusSessionResponse> StopStatusSessionAsync(string token, Guid statusSessionId)
	{
		StopStatusSessionRequest stopStatusSessionRequest = new StopStatusSessionRequest();
		stopStatusSessionRequest.Body = new StopStatusSessionRequestBody();
		stopStatusSessionRequest.Body.token = token;
		stopStatusSessionRequest.Body.statusSessionId = statusSessionId;
		return ((RecorderStatusService2Soap)this).StopStatusSessionAsync(stopStatusSessionRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SubscribeEventStatusResponse RecorderStatusService2Soap.SubscribeEventStatus(SubscribeEventStatusRequest request)
	{
		return base.Channel.SubscribeEventStatus(request);
	}

	public void SubscribeEventStatus(string token, Guid statusSessionId, ArrayOfGuid eventIds)
	{
		SubscribeEventStatusRequest subscribeEventStatusRequest = new SubscribeEventStatusRequest();
		subscribeEventStatusRequest.Body = new SubscribeEventStatusRequestBody();
		subscribeEventStatusRequest.Body.token = token;
		subscribeEventStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeEventStatusRequest.Body.eventIds = eventIds;
		((RecorderStatusService2Soap)this).SubscribeEventStatus(subscribeEventStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<SubscribeEventStatusResponse> RecorderStatusService2Soap.SubscribeEventStatusAsync(SubscribeEventStatusRequest request)
	{
		return base.Channel.SubscribeEventStatusAsync(request);
	}

	public Task<SubscribeEventStatusResponse> SubscribeEventStatusAsync(string token, Guid statusSessionId, ArrayOfGuid eventIds)
	{
		SubscribeEventStatusRequest subscribeEventStatusRequest = new SubscribeEventStatusRequest();
		subscribeEventStatusRequest.Body = new SubscribeEventStatusRequestBody();
		subscribeEventStatusRequest.Body.token = token;
		subscribeEventStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeEventStatusRequest.Body.eventIds = eventIds;
		return ((RecorderStatusService2Soap)this).SubscribeEventStatusAsync(subscribeEventStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SubscribeDeviceStatusResponse RecorderStatusService2Soap.SubscribeDeviceStatus(SubscribeDeviceStatusRequest request)
	{
		return base.Channel.SubscribeDeviceStatus(request);
	}

	public void SubscribeDeviceStatus(string token, Guid statusSessionId, ArrayOfGuid deviceIds)
	{
		SubscribeDeviceStatusRequest subscribeDeviceStatusRequest = new SubscribeDeviceStatusRequest();
		subscribeDeviceStatusRequest.Body = new SubscribeDeviceStatusRequestBody();
		subscribeDeviceStatusRequest.Body.token = token;
		subscribeDeviceStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeDeviceStatusRequest.Body.deviceIds = deviceIds;
		((RecorderStatusService2Soap)this).SubscribeDeviceStatus(subscribeDeviceStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<SubscribeDeviceStatusResponse> RecorderStatusService2Soap.SubscribeDeviceStatusAsync(SubscribeDeviceStatusRequest request)
	{
		return base.Channel.SubscribeDeviceStatusAsync(request);
	}

	public Task<SubscribeDeviceStatusResponse> SubscribeDeviceStatusAsync(string token, Guid statusSessionId, ArrayOfGuid deviceIds)
	{
		SubscribeDeviceStatusRequest subscribeDeviceStatusRequest = new SubscribeDeviceStatusRequest();
		subscribeDeviceStatusRequest.Body = new SubscribeDeviceStatusRequestBody();
		subscribeDeviceStatusRequest.Body.token = token;
		subscribeDeviceStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeDeviceStatusRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).SubscribeDeviceStatusAsync(subscribeDeviceStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SubscribeHardwareStatusResponse RecorderStatusService2Soap.SubscribeHardwareStatus(SubscribeHardwareStatusRequest request)
	{
		return base.Channel.SubscribeHardwareStatus(request);
	}

	public void SubscribeHardwareStatus(string token, Guid statusSessionId, ArrayOfGuid hardwareIds)
	{
		SubscribeHardwareStatusRequest subscribeHardwareStatusRequest = new SubscribeHardwareStatusRequest();
		subscribeHardwareStatusRequest.Body = new SubscribeHardwareStatusRequestBody();
		subscribeHardwareStatusRequest.Body.token = token;
		subscribeHardwareStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeHardwareStatusRequest.Body.hardwareIds = hardwareIds;
		((RecorderStatusService2Soap)this).SubscribeHardwareStatus(subscribeHardwareStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<SubscribeHardwareStatusResponse> RecorderStatusService2Soap.SubscribeHardwareStatusAsync(SubscribeHardwareStatusRequest request)
	{
		return base.Channel.SubscribeHardwareStatusAsync(request);
	}

	public Task<SubscribeHardwareStatusResponse> SubscribeHardwareStatusAsync(string token, Guid statusSessionId, ArrayOfGuid hardwareIds)
	{
		SubscribeHardwareStatusRequest subscribeHardwareStatusRequest = new SubscribeHardwareStatusRequest();
		subscribeHardwareStatusRequest.Body = new SubscribeHardwareStatusRequestBody();
		subscribeHardwareStatusRequest.Body.token = token;
		subscribeHardwareStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeHardwareStatusRequest.Body.hardwareIds = hardwareIds;
		return ((RecorderStatusService2Soap)this).SubscribeHardwareStatusAsync(subscribeHardwareStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	SubscribeConfigurationStatusResponse RecorderStatusService2Soap.SubscribeConfigurationStatus(SubscribeConfigurationStatusRequest request)
	{
		return base.Channel.SubscribeConfigurationStatus(request);
	}

	public void SubscribeConfigurationStatus(string token, Guid statusSessionId, bool subscribe)
	{
		SubscribeConfigurationStatusRequest subscribeConfigurationStatusRequest = new SubscribeConfigurationStatusRequest();
		subscribeConfigurationStatusRequest.Body = new SubscribeConfigurationStatusRequestBody();
		subscribeConfigurationStatusRequest.Body.token = token;
		subscribeConfigurationStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeConfigurationStatusRequest.Body.subscribe = subscribe;
		((RecorderStatusService2Soap)this).SubscribeConfigurationStatus(subscribeConfigurationStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<SubscribeConfigurationStatusResponse> RecorderStatusService2Soap.SubscribeConfigurationStatusAsync(SubscribeConfigurationStatusRequest request)
	{
		return base.Channel.SubscribeConfigurationStatusAsync(request);
	}

	public Task<SubscribeConfigurationStatusResponse> SubscribeConfigurationStatusAsync(string token, Guid statusSessionId, bool subscribe)
	{
		SubscribeConfigurationStatusRequest subscribeConfigurationStatusRequest = new SubscribeConfigurationStatusRequest();
		subscribeConfigurationStatusRequest.Body = new SubscribeConfigurationStatusRequestBody();
		subscribeConfigurationStatusRequest.Body.token = token;
		subscribeConfigurationStatusRequest.Body.statusSessionId = statusSessionId;
		subscribeConfigurationStatusRequest.Body.subscribe = subscribe;
		return ((RecorderStatusService2Soap)this).SubscribeConfigurationStatusAsync(subscribeConfigurationStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetStatusResponse RecorderStatusService2Soap.GetStatus(GetStatusRequest request)
	{
		return base.Channel.GetStatus(request);
	}

	public Status GetStatus(string token, Guid statusSessionId, int millisecondsTimeout)
	{
		GetStatusRequest getStatusRequest = new GetStatusRequest();
		getStatusRequest.Body = new GetStatusRequestBody();
		getStatusRequest.Body.token = token;
		getStatusRequest.Body.statusSessionId = statusSessionId;
		getStatusRequest.Body.millisecondsTimeout = millisecondsTimeout;
		return ((RecorderStatusService2Soap)this).GetStatus(getStatusRequest).Body.GetStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetStatusResponse> RecorderStatusService2Soap.GetStatusAsync(GetStatusRequest request)
	{
		return base.Channel.GetStatusAsync(request);
	}

	public Task<GetStatusResponse> GetStatusAsync(string token, Guid statusSessionId, int millisecondsTimeout)
	{
		GetStatusRequest getStatusRequest = new GetStatusRequest();
		getStatusRequest.Body = new GetStatusRequestBody();
		getStatusRequest.Body.token = token;
		getStatusRequest.Body.statusSessionId = statusSessionId;
		getStatusRequest.Body.millisecondsTimeout = millisecondsTimeout;
		return ((RecorderStatusService2Soap)this).GetStatusAsync(getStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetCurrentDeviceStatusResponse RecorderStatusService2Soap.GetCurrentDeviceStatus(GetCurrentDeviceStatusRequest request)
	{
		return base.Channel.GetCurrentDeviceStatus(request);
	}

	public Status GetCurrentDeviceStatus(string token, ArrayOfGuid deviceIds)
	{
		GetCurrentDeviceStatusRequest getCurrentDeviceStatusRequest = new GetCurrentDeviceStatusRequest();
		getCurrentDeviceStatusRequest.Body = new GetCurrentDeviceStatusRequestBody();
		getCurrentDeviceStatusRequest.Body.token = token;
		getCurrentDeviceStatusRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetCurrentDeviceStatus(getCurrentDeviceStatusRequest).Body.GetCurrentDeviceStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetCurrentDeviceStatusResponse> RecorderStatusService2Soap.GetCurrentDeviceStatusAsync(GetCurrentDeviceStatusRequest request)
	{
		return base.Channel.GetCurrentDeviceStatusAsync(request);
	}

	public Task<GetCurrentDeviceStatusResponse> GetCurrentDeviceStatusAsync(string token, ArrayOfGuid deviceIds)
	{
		GetCurrentDeviceStatusRequest getCurrentDeviceStatusRequest = new GetCurrentDeviceStatusRequest();
		getCurrentDeviceStatusRequest.Body = new GetCurrentDeviceStatusRequestBody();
		getCurrentDeviceStatusRequest.Body.token = token;
		getCurrentDeviceStatusRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetCurrentDeviceStatusAsync(getCurrentDeviceStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetCurrentHardwareStatusResponse RecorderStatusService2Soap.GetCurrentHardwareStatus(GetCurrentHardwareStatusRequest request)
	{
		return base.Channel.GetCurrentHardwareStatus(request);
	}

	public Status GetCurrentHardwareStatus(string token, ArrayOfGuid hardwareIds)
	{
		GetCurrentHardwareStatusRequest getCurrentHardwareStatusRequest = new GetCurrentHardwareStatusRequest();
		getCurrentHardwareStatusRequest.Body = new GetCurrentHardwareStatusRequestBody();
		getCurrentHardwareStatusRequest.Body.token = token;
		getCurrentHardwareStatusRequest.Body.hardwareIds = hardwareIds;
		return ((RecorderStatusService2Soap)this).GetCurrentHardwareStatus(getCurrentHardwareStatusRequest).Body.GetCurrentHardwareStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetCurrentHardwareStatusResponse> RecorderStatusService2Soap.GetCurrentHardwareStatusAsync(GetCurrentHardwareStatusRequest request)
	{
		return base.Channel.GetCurrentHardwareStatusAsync(request);
	}

	public Task<GetCurrentHardwareStatusResponse> GetCurrentHardwareStatusAsync(string token, ArrayOfGuid hardwareIds)
	{
		GetCurrentHardwareStatusRequest getCurrentHardwareStatusRequest = new GetCurrentHardwareStatusRequest();
		getCurrentHardwareStatusRequest.Body = new GetCurrentHardwareStatusRequestBody();
		getCurrentHardwareStatusRequest.Body.token = token;
		getCurrentHardwareStatusRequest.Body.hardwareIds = hardwareIds;
		return ((RecorderStatusService2Soap)this).GetCurrentHardwareStatusAsync(getCurrentHardwareStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetRecorderStatusResponse RecorderStatusService2Soap.GetRecorderStatus(GetRecorderStatusRequest request)
	{
		return base.Channel.GetRecorderStatus(request);
	}

	public AttachAndConnectionState GetRecorderStatus(string token)
	{
		GetRecorderStatusRequest getRecorderStatusRequest = new GetRecorderStatusRequest();
		getRecorderStatusRequest.Body = new GetRecorderStatusRequestBody();
		getRecorderStatusRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).GetRecorderStatus(getRecorderStatusRequest).Body.GetRecorderStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetRecorderStatusResponse> RecorderStatusService2Soap.GetRecorderStatusAsync(GetRecorderStatusRequest request)
	{
		return base.Channel.GetRecorderStatusAsync(request);
	}

	public Task<GetRecorderStatusResponse> GetRecorderStatusAsync(string token)
	{
		GetRecorderStatusRequest getRecorderStatusRequest = new GetRecorderStatusRequest();
		getRecorderStatusRequest.Body = new GetRecorderStatusRequestBody();
		getRecorderStatusRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).GetRecorderStatusAsync(getRecorderStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetRecordingStorageStatusResponse RecorderStatusService2Soap.GetRecordingStorageStatus(GetRecordingStorageStatusRequest request)
	{
		return base.Channel.GetRecordingStorageStatus(request);
	}

	public StorageStatus[] GetRecordingStorageStatus(string token)
	{
		GetRecordingStorageStatusRequest getRecordingStorageStatusRequest = new GetRecordingStorageStatusRequest();
		getRecordingStorageStatusRequest.Body = new GetRecordingStorageStatusRequestBody();
		getRecordingStorageStatusRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).GetRecordingStorageStatus(getRecordingStorageStatusRequest).Body.GetRecordingStorageStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetRecordingStorageStatusResponse> RecorderStatusService2Soap.GetRecordingStorageStatusAsync(GetRecordingStorageStatusRequest request)
	{
		return base.Channel.GetRecordingStorageStatusAsync(request);
	}

	public Task<GetRecordingStorageStatusResponse> GetRecordingStorageStatusAsync(string token)
	{
		GetRecordingStorageStatusRequest getRecordingStorageStatusRequest = new GetRecordingStorageStatusRequest();
		getRecordingStorageStatusRequest.Body = new GetRecordingStorageStatusRequestBody();
		getRecordingStorageStatusRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).GetRecordingStorageStatusAsync(getRecordingStorageStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetArchiveStorageStatusResponse RecorderStatusService2Soap.GetArchiveStorageStatus(GetArchiveStorageStatusRequest request)
	{
		return base.Channel.GetArchiveStorageStatus(request);
	}

	public StorageStatus[] GetArchiveStorageStatus(string token)
	{
		GetArchiveStorageStatusRequest getArchiveStorageStatusRequest = new GetArchiveStorageStatusRequest();
		getArchiveStorageStatusRequest.Body = new GetArchiveStorageStatusRequestBody();
		getArchiveStorageStatusRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).GetArchiveStorageStatus(getArchiveStorageStatusRequest).Body.GetArchiveStorageStatusResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetArchiveStorageStatusResponse> RecorderStatusService2Soap.GetArchiveStorageStatusAsync(GetArchiveStorageStatusRequest request)
	{
		return base.Channel.GetArchiveStorageStatusAsync(request);
	}

	public Task<GetArchiveStorageStatusResponse> GetArchiveStorageStatusAsync(string token)
	{
		GetArchiveStorageStatusRequest getArchiveStorageStatusRequest = new GetArchiveStorageStatusRequest();
		getArchiveStorageStatusRequest.Body = new GetArchiveStorageStatusRequestBody();
		getArchiveStorageStatusRequest.Body.token = token;
		return ((RecorderStatusService2Soap)this).GetArchiveStorageStatusAsync(getArchiveStorageStatusRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetVideoDeviceStatisticsResponse RecorderStatusService2Soap.GetVideoDeviceStatistics(GetVideoDeviceStatisticsRequest request)
	{
		return base.Channel.GetVideoDeviceStatistics(request);
	}

	public VideoDeviceStatistics[] GetVideoDeviceStatistics(string token, ArrayOfGuid deviceIds)
	{
		GetVideoDeviceStatisticsRequest getVideoDeviceStatisticsRequest = new GetVideoDeviceStatisticsRequest();
		getVideoDeviceStatisticsRequest.Body = new GetVideoDeviceStatisticsRequestBody();
		getVideoDeviceStatisticsRequest.Body.token = token;
		getVideoDeviceStatisticsRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetVideoDeviceStatistics(getVideoDeviceStatisticsRequest).Body.GetVideoDeviceStatisticsResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetVideoDeviceStatisticsResponse> RecorderStatusService2Soap.GetVideoDeviceStatisticsAsync(GetVideoDeviceStatisticsRequest request)
	{
		return base.Channel.GetVideoDeviceStatisticsAsync(request);
	}

	public Task<GetVideoDeviceStatisticsResponse> GetVideoDeviceStatisticsAsync(string token, ArrayOfGuid deviceIds)
	{
		GetVideoDeviceStatisticsRequest getVideoDeviceStatisticsRequest = new GetVideoDeviceStatisticsRequest();
		getVideoDeviceStatisticsRequest.Body = new GetVideoDeviceStatisticsRequestBody();
		getVideoDeviceStatisticsRequest.Body.token = token;
		getVideoDeviceStatisticsRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetVideoDeviceStatisticsAsync(getVideoDeviceStatisticsRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetAudioDeviceStatisticsResponse RecorderStatusService2Soap.GetAudioDeviceStatistics(GetAudioDeviceStatisticsRequest request)
	{
		return base.Channel.GetAudioDeviceStatistics(request);
	}

	public AudioDeviceStatistics[] GetAudioDeviceStatistics(string token, ArrayOfGuid deviceIds)
	{
		GetAudioDeviceStatisticsRequest getAudioDeviceStatisticsRequest = new GetAudioDeviceStatisticsRequest();
		getAudioDeviceStatisticsRequest.Body = new GetAudioDeviceStatisticsRequestBody();
		getAudioDeviceStatisticsRequest.Body.token = token;
		getAudioDeviceStatisticsRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetAudioDeviceStatistics(getAudioDeviceStatisticsRequest).Body.GetAudioDeviceStatisticsResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetAudioDeviceStatisticsResponse> RecorderStatusService2Soap.GetAudioDeviceStatisticsAsync(GetAudioDeviceStatisticsRequest request)
	{
		return base.Channel.GetAudioDeviceStatisticsAsync(request);
	}

	public Task<GetAudioDeviceStatisticsResponse> GetAudioDeviceStatisticsAsync(string token, ArrayOfGuid deviceIds)
	{
		GetAudioDeviceStatisticsRequest getAudioDeviceStatisticsRequest = new GetAudioDeviceStatisticsRequest();
		getAudioDeviceStatisticsRequest.Body = new GetAudioDeviceStatisticsRequestBody();
		getAudioDeviceStatisticsRequest.Body.token = token;
		getAudioDeviceStatisticsRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetAudioDeviceStatisticsAsync(getAudioDeviceStatisticsRequest);
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	GetMetadataDeviceStatisticsResponse RecorderStatusService2Soap.GetMetadataDeviceStatistics(GetMetadataDeviceStatisticsRequest request)
	{
		return base.Channel.GetMetadataDeviceStatistics(request);
	}

	public MetadataDeviceStatistics[] GetMetadataDeviceStatistics(string token, ArrayOfGuid deviceIds)
	{
		GetMetadataDeviceStatisticsRequest getMetadataDeviceStatisticsRequest = new GetMetadataDeviceStatisticsRequest();
		getMetadataDeviceStatisticsRequest.Body = new GetMetadataDeviceStatisticsRequestBody();
		getMetadataDeviceStatisticsRequest.Body.token = token;
		getMetadataDeviceStatisticsRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetMetadataDeviceStatistics(getMetadataDeviceStatisticsRequest).Body.GetMetadataDeviceStatisticsResult;
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	Task<GetMetadataDeviceStatisticsResponse> RecorderStatusService2Soap.GetMetadataDeviceStatisticsAsync(GetMetadataDeviceStatisticsRequest request)
	{
		return base.Channel.GetMetadataDeviceStatisticsAsync(request);
	}

	public Task<GetMetadataDeviceStatisticsResponse> GetMetadataDeviceStatisticsAsync(string token, ArrayOfGuid deviceIds)
	{
		GetMetadataDeviceStatisticsRequest getMetadataDeviceStatisticsRequest = new GetMetadataDeviceStatisticsRequest();
		getMetadataDeviceStatisticsRequest.Body = new GetMetadataDeviceStatisticsRequestBody();
		getMetadataDeviceStatisticsRequest.Body.token = token;
		getMetadataDeviceStatisticsRequest.Body.deviceIds = deviceIds;
		return ((RecorderStatusService2Soap)this).GetMetadataDeviceStatisticsAsync(getMetadataDeviceStatisticsRequest);
	}
}
