using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace VideoOS.Mobile.Portable.Net;

public class NetworkInterfacesImplementer : INetworkInterface
{
	public IEnumerable<NetworkInterfaceProperties> GetAllNetworkInterfaces()
	{
		List<NetworkInterfaceProperties> list = new List<NetworkInterfaceProperties>();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			if (networkInterface.Name.Equals(string.Empty) || networkInterface.OperationalStatus != OperationalStatus.Up)
			{
				continue;
			}
			foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
			{
				if (unicastAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || unicastAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
				{
					AddressFamily addressFamily = ((unicastAddress.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork);
					string address = unicastAddress.Address.ToString();
					list.Add(new NetworkInterfaceProperties
					{
						Index = networkInterface.GetIPProperties().GetIPv4Properties().Index,
						Address = new IPAddress
						{
							Address = address,
							AddressFamily = addressFamily
						}
					});
				}
			}
		}
		return list;
	}
}
