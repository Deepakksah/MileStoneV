namespace VideoOS.Management.VmoClient;

public class VmoClientFactory : IVmoClientFactory
{
	public IVmoClient Create(IServerConnection serverConnection)
	{
		return new VmoClient(serverConnection);
	}
}
