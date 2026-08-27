using System;
using System.Globalization;

namespace VideoOS.Management.Common;

public class EndpointDefinition
{
	public string ServicePath { get; set; }

	public AuthenticationType AuthenticationType { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Scheme { get; set; }

	public string GetEndpointUri(ServerAddress address)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		return GetEndpointUri(new Uri(address.URL));
	}

	public string GetEndpointUri(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (string.Compare(Scheme, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}", Scheme, uri.Authority, ServicePath);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}", Scheme, uri.Host, ServicePath);
	}

	public EndpointDefinition()
	{
	}

	public EndpointDefinition(string servicePath, AuthenticationType authenticationType, string name, string description, string scheme)
	{
		ServicePath = servicePath;
		AuthenticationType = authenticationType;
		Name = name;
		Description = description;
		Scheme = scheme;
	}
}
