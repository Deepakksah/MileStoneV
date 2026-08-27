namespace VideoOS.Management.Server.RestApi;

public class ServerWebServiceManagerFactory : IServerWebServiceManagerFactory
{
	public IServerWebServiceManager CreateServerWebServiceManager()
	{
		return new ServerWebServiceManagerWrapper();
	}
}
