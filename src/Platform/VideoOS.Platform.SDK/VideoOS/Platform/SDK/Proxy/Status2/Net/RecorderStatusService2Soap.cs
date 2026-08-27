using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2", ConfigurationName = "RecorderStatusService2Soap")]
public interface RecorderStatusService2Soap
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetVersion", ReplyAction = "*")]
	int GetVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetVersion", ReplyAction = "*")]
	Task<int> GetVersionAsync();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/StartStatusSession", ReplyAction = "*")]
	StartStatusSessionResponse StartStatusSession(StartStatusSessionRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/StartStatusSession", ReplyAction = "*")]
	Task<StartStatusSessionResponse> StartStatusSessionAsync(StartStatusSessionRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/StopStatusSession", ReplyAction = "*")]
	StopStatusSessionResponse StopStatusSession(StopStatusSessionRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/StopStatusSession", ReplyAction = "*")]
	Task<StopStatusSessionResponse> StopStatusSessionAsync(StopStatusSessionRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeEventStatus", ReplyAction = "*")]
	SubscribeEventStatusResponse SubscribeEventStatus(SubscribeEventStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeEventStatus", ReplyAction = "*")]
	Task<SubscribeEventStatusResponse> SubscribeEventStatusAsync(SubscribeEventStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeDeviceStatus", ReplyAction = "*")]
	SubscribeDeviceStatusResponse SubscribeDeviceStatus(SubscribeDeviceStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeDeviceStatus", ReplyAction = "*")]
	Task<SubscribeDeviceStatusResponse> SubscribeDeviceStatusAsync(SubscribeDeviceStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeHardwareStatus", ReplyAction = "*")]
	SubscribeHardwareStatusResponse SubscribeHardwareStatus(SubscribeHardwareStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeHardwareStatus", ReplyAction = "*")]
	Task<SubscribeHardwareStatusResponse> SubscribeHardwareStatusAsync(SubscribeHardwareStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeConfigurationStatus", ReplyAction = "*")]
	SubscribeConfigurationStatusResponse SubscribeConfigurationStatus(SubscribeConfigurationStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/SubscribeConfigurationStatus", ReplyAction = "*")]
	Task<SubscribeConfigurationStatusResponse> SubscribeConfigurationStatusAsync(SubscribeConfigurationStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetStatus", ReplyAction = "*")]
	GetStatusResponse GetStatus(GetStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetStatus", ReplyAction = "*")]
	Task<GetStatusResponse> GetStatusAsync(GetStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetCurrentDeviceStatus", ReplyAction = "*")]
	GetCurrentDeviceStatusResponse GetCurrentDeviceStatus(GetCurrentDeviceStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetCurrentDeviceStatus", ReplyAction = "*")]
	Task<GetCurrentDeviceStatusResponse> GetCurrentDeviceStatusAsync(GetCurrentDeviceStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetCurrentHardwareStatus", ReplyAction = "*")]
	GetCurrentHardwareStatusResponse GetCurrentHardwareStatus(GetCurrentHardwareStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetCurrentHardwareStatus", ReplyAction = "*")]
	Task<GetCurrentHardwareStatusResponse> GetCurrentHardwareStatusAsync(GetCurrentHardwareStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetRecorderStatus", ReplyAction = "*")]
	GetRecorderStatusResponse GetRecorderStatus(GetRecorderStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetRecorderStatus", ReplyAction = "*")]
	Task<GetRecorderStatusResponse> GetRecorderStatusAsync(GetRecorderStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetRecordingStorageStatus", ReplyAction = "*")]
	GetRecordingStorageStatusResponse GetRecordingStorageStatus(GetRecordingStorageStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetRecordingStorageStatus", ReplyAction = "*")]
	Task<GetRecordingStorageStatusResponse> GetRecordingStorageStatusAsync(GetRecordingStorageStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetArchiveStorageStatus", ReplyAction = "*")]
	GetArchiveStorageStatusResponse GetArchiveStorageStatus(GetArchiveStorageStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetArchiveStorageStatus", ReplyAction = "*")]
	Task<GetArchiveStorageStatusResponse> GetArchiveStorageStatusAsync(GetArchiveStorageStatusRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetVideoDeviceStatistics", ReplyAction = "*")]
	GetVideoDeviceStatisticsResponse GetVideoDeviceStatistics(GetVideoDeviceStatisticsRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetVideoDeviceStatistics", ReplyAction = "*")]
	Task<GetVideoDeviceStatisticsResponse> GetVideoDeviceStatisticsAsync(GetVideoDeviceStatisticsRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetAudioDeviceStatistics", ReplyAction = "*")]
	GetAudioDeviceStatisticsResponse GetAudioDeviceStatistics(GetAudioDeviceStatisticsRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetAudioDeviceStatistics", ReplyAction = "*")]
	Task<GetAudioDeviceStatisticsResponse> GetAudioDeviceStatisticsAsync(GetAudioDeviceStatisticsRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetMetadataDeviceStatistics", ReplyAction = "*")]
	GetMetadataDeviceStatisticsResponse GetMetadataDeviceStatistics(GetMetadataDeviceStatisticsRequest request);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSRecorderStatus2/GetMetadataDeviceStatistics", ReplyAction = "*")]
	Task<GetMetadataDeviceStatisticsResponse> GetMetadataDeviceStatisticsAsync(GetMetadataDeviceStatisticsRequest request);
}
