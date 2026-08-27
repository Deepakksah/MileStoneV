using System.Net;
using VideoOS.Utilities.Network;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class IPHelperWrapper : IIPHelperWrapper
{
	public string GetHostName()
	{
		return IPHelper.DnsGetHostEntry(Dns.GetHostName()).HostName;
	}
}
