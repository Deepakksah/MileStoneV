using System.Net;

namespace VideoOS.Mobile.Portable.Net;

public class MulticastClientFactory : IMulticastClientFactory
{
	public static IMulticastClientFactory Instance { get; } = new MulticastClientFactory();

	public IMulticastClient CreateMulticastClient(string ipAddress, string multicastAddress, int index)
	{
		return CreateMulticastClient(ipAddress, 0, multicastAddress, index);
	}

	public IMulticastClient CreateMulticastClient(string ipAddress, int port, string multicastAddress, int index)
	{
		IPEndPoint localEndPoint = new IPEndPoint(System.Net.IPAddress.Parse(ipAddress), port);
		System.Net.IPAddress multicastaddress = System.Net.IPAddress.Parse(multicastAddress);
		return new MulticastClientImplementer(localEndPoint, multicastaddress, index);
	}
}
