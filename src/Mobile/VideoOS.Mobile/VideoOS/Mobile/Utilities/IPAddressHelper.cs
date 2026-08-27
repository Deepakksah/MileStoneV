using System.Net;
using System.Net.Sockets;

namespace VideoOS.Mobile.Utilities;

public static class IPAddressHelper
{
	public static string GetIpAddress(this IPAddress address)
	{
		if (address.AddressFamily == AddressFamily.InterNetworkV6)
		{
			string oldValue = $"%{address.ScopeId}";
			return address.ToString().Replace(oldValue, "");
		}
		return address.ToString();
	}
}
