using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace VideoOS.Utilities.Network;

public static class IPHelper
{
	public struct MIB_IPFORWARDROW
	{
		public uint dwForwardDest;

		public uint dwForwardMask;

		public uint dwForwardPolicy;

		public uint dwForwardNextHop;

		public uint dwForwardIfIndex;

		public uint dwForwardType;

		public uint dwForwardProto;

		public uint dwForwardAge;

		public uint dwForwardNextHopAS;

		public uint dwForwardMetric1;

		public uint dwForwardMetric2;

		public uint dwForwardMetric3;

		public uint dwForwardMetric4;

		public uint dwForwardMetric5;
	}

	public enum IPValidator
	{
		ipOK,
		InvalidCharacter,
		InvalidAddress
	}

	private const string IPv4_ADDRESS_REGEX = "^(?<IPv4>\\d+\\.\\d+\\.\\d+\\.\\d+)(:(?<Port>\\d+?))?$";

	private const string IPv6_SQUARED_ADDRESS_REGEX = "^\\[(?<IPv6>[^\\]]+)\\](:(?<Port>\\d+))?$";

	private const string IPv6_ADDRESS_REGEX = "^(?<IPv6>(.*:){2,7}.*)$";

	private const string HOSTNAME_ADDRESS_REGEX = "^(?<host>[a-zA-Z0-9\\.-]+)(:(?<Port>\\d+?))?$";

	public static IPHostEntry DnsGetHostEntry(string hostNameOrAddress)
	{
		IPAddress address = null;
		if (!IPAddress.TryParse(hostNameOrAddress, out address))
		{
			return Dns.GetHostEntry(hostNameOrAddress);
		}
		if (address.AddressFamily == AddressFamily.InterNetwork)
		{
			return Dns.Resolve(hostNameOrAddress);
		}
		return Dns.GetHostEntry(hostNameOrAddress);
	}

	public static IPAddress ConvertUriToIPAddress(Uri uri)
	{
		switch (uri.HostNameType)
		{
		case UriHostNameType.IPv4:
		case UriHostNameType.IPv6:
			return IPAddress.Parse(uri.DnsSafeHost);
		case UriHostNameType.Basic:
		case UriHostNameType.Dns:
		{
			IPAddress[] addressList = DnsGetHostEntry(uri.DnsSafeHost).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork || iPAddress.AddressFamily == AddressFamily.InterNetworkV6)
				{
					return iPAddress;
				}
			}
			break;
		}
		}
		throw new ArgumentException("Unable to convert given uri to IP address");
	}

	private static IPAddress GetFirstRouteIPAddress(IPAddress remoteAddress, int port)
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			if ((remoteAddress.AddressFamily == AddressFamily.InterNetwork && !networkInterface.Supports(NetworkInterfaceComponent.IPv4)) || (remoteAddress.AddressFamily == AddressFamily.InterNetworkV6 && !networkInterface.Supports(NetworkInterfaceComponent.IPv6)))
			{
				continue;
			}
			UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
			if (unicastAddresses == null)
			{
				continue;
			}
			foreach (UnicastIPAddressInformation item in unicastAddresses)
			{
				if (item.Address.AddressFamily == remoteAddress.AddressFamily && item.IsDnsEligible)
				{
					IPEndPoint localEP = new IPEndPoint(item.Address, 0);
					IPEndPoint remoteEP = new IPEndPoint(remoteAddress, port);
					try
					{
						Socket socket = new Socket(remoteAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
						socket.Bind(localEP);
						socket.Connect(remoteEP);
						return item.Address;
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception)
					{
					}
				}
			}
		}
		throw new ArgumentException("Unable to find best interface IP address for remote address: " + remoteAddress.ToString());
	}

	public static IPAddress GetInterfaceIPAddress(Uri remoteUri)
	{
		return GetInterfaceIPAddress(ConvertUriToIPAddress(remoteUri), remoteUri.Port);
	}

	public static IPAddress GetInterfaceIPAddress(IPAddress remoteAddress, int port)
	{
		return GetFirstRouteIPAddress(remoteAddress, port);
	}

	public static bool IsNatRequest(string remoteAddress, string serverHostAddress)
	{
		if (!IPAddress.TryParse(remoteAddress, out var address))
		{
			return false;
		}
		if (IPAddress.IsLoopback(address))
		{
			return false;
		}
		if (address.AddressFamily == AddressFamily.InterNetworkV6 && (address.IsIPv6LinkLocal || address.IsIPv6SiteLocal || address.IsIPv6Multicast))
		{
			return false;
		}
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		if (allNetworkInterfaces.Length < 1)
		{
			return false;
		}
		UnicastIPAddressInformation unicastIPAddressInformation = null;
		NetworkInterface[] array = allNetworkInterfaces;
		for (int i = 0; i < array.Length; i++)
		{
			IPInterfaceProperties iPProperties = array[i].GetIPProperties();
			unicastIPAddressInformation = ((address.AddressFamily != AddressFamily.InterNetworkV6) ? GetAddressIfMatchIPv4(iPProperties, serverHostAddress) : GetAddressIfMatchIPv6(iPProperties, serverHostAddress));
			if (unicastIPAddressInformation != null)
			{
				break;
			}
		}
		if (unicastIPAddressInformation == null)
		{
			return false;
		}
		if (address.AddressFamily == AddressFamily.InterNetworkV6)
		{
			return IsNatRequestIPv6(address, unicastIPAddressInformation);
		}
		return IsNatRequestIPv4(address, unicastIPAddressInformation);
	}

	private static bool IsNatRequestIPv6(IPAddress remoteIPAddress, UnicastIPAddressInformation localAddress)
	{
		string[] array = remoteIPAddress.ToString().Split(':');
		string[] array2 = localAddress.Address.ToString().Split(':');
		if (array.Length < 4 || array2.Length < 4)
		{
			return false;
		}
		return string.Compare(array[3], array2[3], ignoreCase: true, CultureInfo.InvariantCulture) != 0;
	}

	private static bool IsNatRequestIPv4(IPAddress remoteIPAddress, UnicastIPAddressInformation localAddress)
	{
		long num = ~ConvertBytesToLong(localAddress.IPv4Mask.GetAddressBytes());
		long num2 = ConvertBytesToLong(localAddress.Address.GetAddressBytes()) | num;
		long num3 = ConvertBytesToLong(remoteIPAddress.GetAddressBytes()) | num;
		if (num2 != num3)
		{
			return true;
		}
		return false;
	}

	private static UnicastIPAddressInformation GetAddressIfMatchIPv4(IPInterfaceProperties interfaceProperties, string serverHostAddress)
	{
		foreach (UnicastIPAddressInformation unicastAddress in interfaceProperties.UnicastAddresses)
		{
			if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork && unicastAddress.IPv4Mask != null)
			{
				if (!IPAddress.TryParse(serverHostAddress, out var address))
				{
					return unicastAddress;
				}
				if (IPAddressCompare(address, unicastAddress.Address) == 0)
				{
					return unicastAddress;
				}
			}
		}
		return null;
	}

	private static UnicastIPAddressInformation GetAddressIfMatchIPv6(IPInterfaceProperties interfaceProperties, string serverHostAddress)
	{
		foreach (UnicastIPAddressInformation unicastAddress in interfaceProperties.UnicastAddresses)
		{
			if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				if (!IPAddress.TryParse(serverHostAddress, out var address))
				{
					return unicastAddress;
				}
				if (IPAddressCompare(address, unicastAddress.Address) == 0)
				{
					return unicastAddress;
				}
			}
		}
		return null;
	}

	public static IPValidator ValidateIPorHost(string address)
	{
		if (address.Contains("/") || address.StartsWith("http://") || address.StartsWith("https://"))
		{
			return IPValidator.InvalidCharacter;
		}
		if (IPAddress.TryParse(address, out var address2))
		{
			if (address2.AddressFamily == AddressFamily.InterNetwork && address.Split('.').Length != 4)
			{
				return IPValidator.InvalidAddress;
			}
			return IPValidator.ipOK;
		}
		string text = address.ToLower();
		for (int i = 0; i < text.Length; i++)
		{
			if (('a' > text[i] || text[i] > 'z') && ('0' > text[i] || text[i] > '9') && text[i] != '.' && text[i] != '-')
			{
				return IPValidator.InvalidAddress;
			}
		}
		return IPValidator.ipOK;
	}

	public static bool IsValidHostName(string hostName, HostNameValidationType validationType)
	{
		if (hostName == null)
		{
			throw new ArgumentNullException("hostName");
		}
		if (hostName.Length == 0)
		{
			throw new ArgumentException("hostName");
		}
		if (hostName.Length > 255)
		{
			return false;
		}
		switch (validationType)
		{
		case HostNameValidationType.Strict:
		{
			string[] array = hostName.Split('.');
			foreach (string text in array)
			{
				if (text.Length < 1 || text.Length > 63)
				{
					return false;
				}
				if (text.StartsWith("-", StringComparison.OrdinalIgnoreCase) || text.EndsWith("-", StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				for (int j = 0; j < text.Length; j++)
				{
					_ = text[j];
					if (('a' > text[j] || text[j] > 'z') && ('A' > text[j] || text[j] > 'Z') && ('0' > text[j] || text[j] > '9') && text[j] != '-')
					{
						return false;
					}
				}
			}
			break;
		}
		case HostNameValidationType.Relaxed:
			if (hostName.IndexOfAny(" /\\\\[]:|<>+=;,?$%#@{}^".ToCharArray()) != -1)
			{
				return false;
			}
			try
			{
				GC.KeepAlive(new UriBuilder("http", hostName));
			}
			catch (UriFormatException)
			{
				return false;
			}
			break;
		default:
			throw new ArgumentException("Unknown validatio type specified", "validationType");
		}
		return true;
	}

	public static int IPAddressCompare(IPAddress addressFrom, IPAddress addressTo)
	{
		if (addressFrom.AddressFamily != addressTo.AddressFamily)
		{
			throw new ArgumentException("Cannot compare IPAddress for two addresse with different AddressFamily values");
		}
		byte[] addressBytes = addressFrom.GetAddressBytes();
		byte[] addressBytes2 = addressTo.GetAddressBytes();
		for (int i = 0; i < addressBytes.Length; i++)
		{
			if (addressBytes[i] != addressBytes2[i])
			{
				if (addressBytes[i] <= addressBytes2[i])
				{
					return -1;
				}
				return 1;
			}
		}
		return 0;
	}

	public static IPAddress IncrementIPAddress(IPAddress addr)
	{
		byte[] addressBytes = addr.GetAddressBytes();
		for (int num = addressBytes.Length - 1; num >= 0; num--)
		{
			if (addressBytes[num] != byte.MaxValue)
			{
				addressBytes[num]++;
				break;
			}
			addressBytes[num] = 0;
		}
		return new IPAddress(addressBytes);
	}

	public static IPAddress DecrementIPAddress(IPAddress addr)
	{
		byte[] addressBytes = addr.GetAddressBytes();
		for (int num = addressBytes.Length - 1; num >= 0; num--)
		{
			if (addressBytes[num] != 0)
			{
				addressBytes[num]--;
				break;
			}
			addressBytes[num] = byte.MaxValue;
		}
		return new IPAddress(addressBytes);
	}

	private static long ConvertBytesToLong(byte[] mask)
	{
		long num = 0L;
		foreach (byte value in mask)
		{
			num <<= 8;
			num += Convert.ToInt32(value);
		}
		return num;
	}

	public static string GetMacAddress()
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		if (allNetworkInterfaces == null || allNetworkInterfaces.Length < 1)
		{
			return "";
		}
		return allNetworkInterfaces[0].GetPhysicalAddress().ToString().ToUpperInvariant();
	}

	public static string GetFullCumputerName()
	{
		string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
		string hostName = Dns.GetHostName();
		string text = "";
		if (!hostName.Contains(domainName))
		{
			return hostName + "." + domainName;
		}
		return hostName;
	}

	public static bool ValidateAddress(string address, out string hostNameOrIP, out int port)
	{
		port = 0;
		hostNameOrIP = "0.0.0.0";
		if (string.IsNullOrEmpty(address))
		{
			throw new ArgumentNullException("Input address is null or empty");
		}
		if (address.Contains("/") || address.StartsWith("http://") || address.StartsWith("https://"))
		{
			return false;
		}
		try
		{
			if (Regex.IsMatch(address, "^(?<IPv4>\\d+\\.\\d+\\.\\d+\\.\\d+)(:(?<Port>\\d+?))?$"))
			{
				string text = null;
				int num = 0;
				Match match = Regex.Match(address, "^(?<IPv4>\\d+\\.\\d+\\.\\d+\\.\\d+)(:(?<Port>\\d+?))?$");
				text = match.Groups["IPv4"].Value;
				if (match.Groups["Port"].Length != 0)
				{
					num = int.Parse(match.Groups["Port"].Value);
				}
				if (IPAddress.TryParse(text, out var address2))
				{
					if (address2.AddressFamily == AddressFamily.InterNetwork && address.Split('.').Length != 4)
					{
						return false;
					}
					hostNameOrIP = address2.ToString();
					port = num;
					return true;
				}
				return false;
			}
			if (Regex.IsMatch(address, "^\\[(?<IPv6>[^\\]]+)\\](:(?<Port>\\d+))?$"))
			{
				string text2 = null;
				int num2 = 0;
				Match match2 = Regex.Match(address, "^\\[(?<IPv6>[^\\]]+)\\](:(?<Port>\\d+))?$");
				text2 = match2.Groups["IPv6"].Value;
				if (match2.Groups["Port"].Length != 0)
				{
					num2 = int.Parse(match2.Groups["Port"].Value);
				}
				if (IPAddress.TryParse(text2, out var address3))
				{
					if (address3.AddressFamily == AddressFamily.InterNetwork && address.Split('.').Length != 4)
					{
						return false;
					}
					hostNameOrIP = string.Format(CultureInfo.InvariantCulture, "[{0}]", address3);
					port = num2;
					return true;
				}
				return false;
			}
			if (Regex.IsMatch(address, "^(?<IPv6>(.*:){2,7}.*)$"))
			{
				int num3 = 0;
				if (IPAddress.TryParse(Regex.Match(address, "^(?<IPv6>(.*:){2,7}.*)$").Groups["IPv6"].Value, out var address4))
				{
					if (address4.AddressFamily == AddressFamily.InterNetwork && address.Split('.').Length != 4)
					{
						return false;
					}
					hostNameOrIP = string.Format(CultureInfo.InvariantCulture, "[{0}]", address4);
					port = num3;
					return true;
				}
				return false;
			}
			if (Regex.IsMatch(address, "^(?<host>[a-zA-Z0-9\\.-]+)(:(?<Port>\\d+?))?$"))
			{
				string text3 = null;
				int num4 = 0;
				Match match3 = Regex.Match(address, "^(?<host>[a-zA-Z0-9\\.-]+)(:(?<Port>\\d+?))?$");
				text3 = match3.Groups["host"].Value;
				if (match3.Groups["Port"].Length != 0)
				{
					num4 = int.Parse(match3.Groups["Port"].Value);
				}
				if (ValidateIPorHost(text3) == IPValidator.ipOK)
				{
					hostNameOrIP = text3;
					port = num4;
					return true;
				}
				return false;
			}
			return false;
		}
		catch (ArgumentNullException)
		{
			throw;
		}
		catch (ArgumentException)
		{
			throw;
		}
	}
}
