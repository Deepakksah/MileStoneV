using System;
using System.Net.Sockets;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class WANIPSanitizer
{
	public const string ValidRemoteHost = "*";

	public bool TrySanitize(PortMapping mapping)
	{
		bool flag = (string.IsNullOrEmpty(mapping.NewInternalClient) || Uri.CheckHostName(mapping.NewInternalClient) != UriHostNameType.Unknown) && (string.IsNullOrEmpty(mapping.NewRemoteHost) || mapping.NewRemoteHost.Equals("*") || Uri.CheckHostName(mapping.NewRemoteHost) != UriHostNameType.Unknown) && (string.IsNullOrEmpty(mapping.NewProtocol) || mapping.NewProtocol.Equals(ProtocolType.Tcp.ToString(), StringComparison.CurrentCultureIgnoreCase) || mapping.NewProtocol.Equals(ProtocolType.Udp.ToString(), StringComparison.CurrentCultureIgnoreCase));
		if (flag && !string.IsNullOrEmpty(mapping.NewPortMappingDescription))
		{
			flag = !string.IsNullOrEmpty(mapping.NewPortMappingDescription.Sanitized());
		}
		return flag;
	}

	public bool TrySanitize(DeletePortMapping mapping)
	{
		if (string.IsNullOrEmpty(mapping.NewProtocol) || mapping.NewProtocol.Equals(ProtocolType.Tcp.ToString(), StringComparison.CurrentCultureIgnoreCase) || mapping.NewProtocol.Equals(ProtocolType.Udp.ToString(), StringComparison.CurrentCultureIgnoreCase))
		{
			if (!string.IsNullOrEmpty(mapping.NewRemoteHost))
			{
				return Uri.CheckHostName(mapping.NewRemoteHost) != UriHostNameType.Unknown;
			}
			return true;
		}
		return false;
	}
}
