namespace VideoOS.Management.Server;

public interface IGeminiServerService
{
	void StartService(IGeminiRequestContext requestContext);

	void StopService(IGeminiRequestContext requestContext);
}
