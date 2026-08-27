namespace VideoOS.Mobile.Portable.Net;

public interface IMulticastClientFactory
{
	IMulticastClient CreateMulticastClient(string ipAddress, string multicastAddress, int index);

	IMulticastClient CreateMulticastClient(string ipAddress, int port, string multicastAddress, int index);
}
