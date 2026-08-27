namespace VideoOS.Management.VmoClient;

public interface IVmoClientFactory
{
	IVmoClient Create(IServerConnection serverConnection);
}
