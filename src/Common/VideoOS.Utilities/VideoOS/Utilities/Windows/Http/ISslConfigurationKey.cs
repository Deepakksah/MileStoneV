using System.Net;

namespace VideoOS.Utilities.Windows.Http;

public interface ISslConfigurationKey
{
	IPAddress Address { get; }

	ushort Port { get; }
}
