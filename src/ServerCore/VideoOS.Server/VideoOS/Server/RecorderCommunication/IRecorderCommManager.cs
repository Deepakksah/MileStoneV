using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderCommManager : IRecorderComm, IServerComm, IServiceConnectionStatusProvider
{
	int MaxSimultaniousRecorderConfigRequests { get; }

	event EventHandler<ResponseEventArgs> ResponseReceived;

	event EventHandler<RecorderCommunicationStateEventArgs> CommunicationStateChanged;

	event EventHandler<RecorderStatusEventArgs> RecorderStatusChanged;

	void Initialize(ServerConfig serverConfig);

	void Close(ServerConfig serverConfig);

	bool RequestGetCompleteConfigurationWork();

	void ReleaseGetCompleteConfigurationWork();

	bool RequestAttachRecorderWork();

	void ReleaseAttachRecorderWork();

	void UnregisterRecorder(Guid serviceId);

	bool RegisterRecorder(IRecorderData recorderData, IRecorderEntryFactory factory, string recorderName, string hostName, string webServerUri, int failoverPort);

	void SetRecorderInformation(Guid serviceId, string displayName, string hostName, Uri webServerUri, string scanRanges, string timeZone, string devicePackInfo, ushort failoverServerPort, int databasePort, Guid failoverId, string version);

	RecorderStatusInfo[] GetRecorderStatus(Guid recorderId, Guid serviceId);

	RecorderCompatibilityInfo GetRecorderCompatibilityStatus(Guid serviceId);

	bool IsRecorderAttachedAndConnected(Guid serviceId);

	bool IsRecorderOnline(Guid recorderId);

	bool IsRecorderNotDisconnected(Guid recorderId);

	bool IsRecorderAvailable(Guid recorderId);

	bool IsRecorderServiceAvailable(Guid serviceId);

	bool RecorderServerServiceExists(Guid recorderId);

	bool IsRecorderHotStandByFailoverType(Guid serviceId);

	bool IsRecorderIncompatible(Guid recorderId);

	bool IsRecorderAttachedAndConnectedNotInFailover(Guid recorderId);

	Guid[] GetConnectedRecordingServerServiceIds();

	Guid GetRecorderId(Guid serviceId);

	Guid[] QueryRecorderServiceIds(Guid recorderId);

	Guid[] QueryRecorderServiceIds();

	Guid QueryActiveRecorderServiceId(Guid recorderId);

	Guid[] QueryConnectedRecorderIds();

	Guid[] QueryHotstandbyServiceIds(Guid recorderId);

	Guid QueryRecorderServiceId(Guid recorderId);

	void SetRecorderStatus(Guid serviceId, RecorderStatuses status);

	RecorderStatusInfo[] GetRecorderStatusAllRecorders();
}
