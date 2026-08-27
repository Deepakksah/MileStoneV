using System.Net;

namespace VideoOS.Utilities.Windows.Http.Internal;

public class SslConfigurationKey
{
	public ushort Port { get; }

	public IPAddress Address { get; }

	public SslConfigurationKey(IPAddress address, ushort port)
	{
		Port = port;
		Address = address;
	}
}
