namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class RecordingServerProxy : IRecordingServerProxy
{
	public RecordingServer QueryRecordingServer(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return RecordingServer.QueryRecordingServer(requestContext, recordingServerId);
	}
}
