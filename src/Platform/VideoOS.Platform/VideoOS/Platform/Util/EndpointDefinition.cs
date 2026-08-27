using System;
using System.Globalization;

namespace VideoOS.Platform.Util;

public class EndpointDefinition
{
	public string ServicePath { get; set; }

	public string AuthenticationType { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Protocol { get; set; }

	public Uri ServerProvidedUri { get; set; }

	public string GetEndpointUri(string address)
	{
		if (ServerProvidedUri != null)
		{
			return ServerProvidedUri.ToString();
		}
		return GetEndpointUri(new Uri(address));
	}

	public string GetEndpointUri(Uri uri)
	{
		string arg = ((Protocol == Uri.UriSchemeHttps) ? uri.Host : uri.Authority);
		return string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}", Protocol.ToString(), arg, ServicePath);
	}

	public EndpointDefinition()
	{
	}

	public EndpointDefinition(string url, string authenticationType, string name, string description)
	{
		Uri uri = new UriBuilder(url).Uri;
		ServicePath = uri.Fragment;
		AuthenticationType = authenticationType;
		Name = name;
		Description = description;
		Protocol = uri.Scheme;
		ServerProvidedUri = uri;
	}
}
