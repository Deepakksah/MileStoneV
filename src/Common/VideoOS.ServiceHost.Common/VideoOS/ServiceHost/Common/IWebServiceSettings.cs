namespace VideoOS.ServiceHost.Common;

public interface IWebServiceSettings
{
	int WebServicePort { get; }

	int ConcurrentConnections { get; }

	int ReceiveTimeout { get; }
}
