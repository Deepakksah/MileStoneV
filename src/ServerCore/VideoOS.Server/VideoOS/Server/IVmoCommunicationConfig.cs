namespace VideoOS.Server;

public interface IVmoCommunicationConfig
{
	string ServerName { get; }

	int Port { get; }

	int HttpsPort { get; }
}
