using System.Net;

namespace VideoOS.GatewayService.Utilities;

public class DnsHelper : IDnsHelper
{
	public string GetDnsHostName(string machineName)
	{
		return Dns.GetHostEntry(machineName).HostName;
	}
}
