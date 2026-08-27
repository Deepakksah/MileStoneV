using System;

namespace VideoOS.Mobile.ReverseProxy;

public interface IServiceDiscovery
{
	Uri GetServiceUri();
}
