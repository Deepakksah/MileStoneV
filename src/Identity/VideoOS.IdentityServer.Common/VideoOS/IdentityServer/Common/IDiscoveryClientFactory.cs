using System;
using System.Collections.Generic;
using System.Net.Http;

namespace VideoOS.IdentityServer.Common;

public interface IDiscoveryClientFactory
{
	IDiscoveryClient Create(IEnumerable<Uri> addresses, HttpMessageHandler innerHandler = null);
}
