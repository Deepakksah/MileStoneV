using System;
using System.Net;
using System.Net.Sockets;

namespace VideoOS.Mobile.Portable.Net;

public class MulticastClientImplementer : UdpClient, IMulticastClient
{
	public string LocalAddress
	{
		get
		{
			string result = null;
			if (base.Client.LocalEndPoint is IPEndPoint iPEndPoint)
			{
				result = iPEndPoint.Address.ToString();
			}
			return result;
		}
	}

	public new short Ttl
	{
		get
		{
			return base.Ttl;
		}
		set
		{
			base.Ttl = value;
		}
	}

	public IPAddress MulticastAddress { get; }

	public MulticastClientImplementer(IPEndPoint localEndPoint, System.Net.IPAddress multicastaddress, int networkAdapterIndex)
		: base(localEndPoint.AddressFamily)
	{
		MulticastAddress = new IPAddress
		{
			Address = multicastaddress.ToString(),
			AddressFamily = ((multicastaddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork)
		};
		base.ExclusiveAddressUse = false;
		base.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
		base.ExclusiveAddressUse = false;
		base.Client.Bind(localEndPoint);
		switch (localEndPoint.AddressFamily)
		{
		case System.Net.Sockets.AddressFamily.InterNetwork:
			JoinMulticastGroup(multicastaddress, localEndPoint.Address);
			base.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, System.Net.IPAddress.HostToNetworkOrder(networkAdapterIndex));
			break;
		case System.Net.Sockets.AddressFamily.InterNetworkV6:
		{
			IPv6MulticastOption pv6MulticastOption = new IPv6MulticastOption(multicastaddress);
			JoinMulticastGroup(networkAdapterIndex, pv6MulticastOption.Group);
			break;
		}
		}
	}

	public new void BeginReceive(AsyncCallback requestCallback, object state)
	{
		base.BeginReceive(requestCallback, state);
	}

	public void Send(byte[] data, int length, int port)
	{
		base.Send(data, length, MulticastAddress.Address, port);
	}

	public new void Send(byte[] data, int length, string ipAddress, int port)
	{
		base.Send(data, length, ipAddress, port);
	}

	public byte[] EndReceive(IAsyncResult result, ref string remote)
	{
		IPEndPoint remoteEP = null;
		byte[] result2 = EndReceive(result, ref remoteEP);
		remote = remoteEP?.ToString();
		return result2;
	}

	void IMulticastClient.Close()
	{
		Close();
	}
}
