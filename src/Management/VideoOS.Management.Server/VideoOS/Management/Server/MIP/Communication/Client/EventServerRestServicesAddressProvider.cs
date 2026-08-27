using System;
using System.Linq;
using VideoOS.Common.Integration.Extensions;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.MIP.Communication.Client;

public static class EventServerRestServicesAddressProvider
{
	public static ServerAddress GetRestServicesAddress(this RegisteredServer registeredServer)
	{
		if (registeredServer == null || registeredServer.Addresses == null || !registeredServer.Addresses.Any())
		{
			return null;
		}
		ServerAddress obj = (registeredServer.Addresses.Any((ServerAddress x) => x.Scheme == Uri.UriSchemeHttps) ? registeredServer.Addresses.FirstOrDefault((ServerAddress x) => x.Scheme == Uri.UriSchemeHttps) : registeredServer.Addresses.FirstOrDefault());
		Uri uri = new Uri(obj.URL);
		uri = uri.SetPort(uri.Port + 1);
		obj.URL = uri.ToString();
		return obj;
	}
}
