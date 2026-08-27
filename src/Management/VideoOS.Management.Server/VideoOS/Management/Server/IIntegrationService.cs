using System;

namespace VideoOS.Management.Server;

public interface IIntegrationService
{
	void TryNotifyServerAsync();

	void TryNotifyRecordingServerAsync(string recordingServerId);

	void TryNotifyRecordingServerAsync(Guid recordingServerId);
}
