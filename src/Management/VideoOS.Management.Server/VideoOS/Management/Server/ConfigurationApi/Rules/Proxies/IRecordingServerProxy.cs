namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public interface IRecordingServerProxy
{
	RecordingServer QueryRecordingServer(IGeminiRequestContext requestContext, string recordingServerId);
}
