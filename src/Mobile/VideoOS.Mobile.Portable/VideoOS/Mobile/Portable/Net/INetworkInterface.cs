using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Net;

public interface INetworkInterface
{
	IEnumerable<NetworkInterfaceProperties> GetAllNetworkInterfaces();
}
