using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class ServerCollection : Collection<Server>
{
	public Server Add(string hostingType, string address, int port)
	{
		Server server = new Server(hostingType, address, port);
		Add(server);
		return server;
	}

	public Server Add(string hostingType, string address, int port, string userName, string password)
	{
		Server server = new Server(hostingType, address, port, userName, password);
		Add(server);
		return server;
	}
}
