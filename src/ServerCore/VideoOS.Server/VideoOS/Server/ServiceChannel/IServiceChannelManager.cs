using System;
using System.Threading.Tasks;

namespace VideoOS.Server.ServiceChannel;

public interface IServiceChannelManager
{
	Task SendHardwareMovedMessageAsync(Guid hardwareId, Guid sourceRecordingServerId, Guid destinationRecordingServerId);

	Task SendRecordingServerActivatedMessageAsync(Guid recorderId, bool isCreated);

	void SendEdgeJobUpdated(Guid deviceId, DateTime startTime, DateTime endTime);

	void SendManualRecordingStateMessage(Guid deviceId, bool isManualRecording);

	void SendRecordingsChangedMessage(Guid deviceId, DateTime startTime, DateTime endTime);

	void SendRetrievalTaskChangedMessage(long taskId, EventAction eventAction);
}
