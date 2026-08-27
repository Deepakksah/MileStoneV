using System.Collections.Generic;

namespace VideoOS.IdentityServer.Configuration;

public class ProxySettings
{
	public int ForwardLimit { get; set; } = 1;

	public List<string> KnownProxies { get; set; } = new List<string>();

	public Dictionary<string, int> KnownNetworks { get; set; } = new Dictionary<string, int>();
}
