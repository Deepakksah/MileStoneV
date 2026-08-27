using System;
using System.Collections.Generic;
using System.Net.Http;

namespace VideoOS.IdentityServer.Common;

public class DiscoveryClientFactory : IDiscoveryClientFactory
{
	public IDiscoveryClient Create(IEnumerable<Uri> addresses, HttpMessageHandler innerHandler = null)
	{
		return new DiscoveryClient(addresses, innerHandler);
	}
}
