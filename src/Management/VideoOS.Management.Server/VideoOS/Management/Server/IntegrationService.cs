using System;

namespace VideoOS.Management.Server;

public sealed class IntegrationService : IIntegrationService, IGeminiServerService
{
	public void TryNotifyServerAsync()
	{
		Integration.TryNotifyServerAsync();
	}

	public void TryNotifyRecordingServerAsync(string recordingServerId)
	{
		Integration.TryNotifyRecordingServerAsync(recordingServerId);
	}

	public void TryNotifyRecordingServerAsync(Guid recordingServerId)
	{
		Integration.TryNotifyRecordingServerAsync(recordingServerId.ToString());
	}

	void IGeminiServerService.StartService(IGeminiRequestContext systemRequestContext)
	{
	}

	void IGeminiServerService.StopService(IGeminiRequestContext systemRequestContext)
	{
	}
}
