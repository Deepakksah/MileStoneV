using System;

namespace VideoOS.Management.Common;

public class ServerAddress
{
	public string URL { get; set; }

	public string Scheme => new Uri(URL).Scheme.ToUpperInvariant();

	public ServerAddressType Type { get; set; }

	public ServerAddress(string url, ServerAddressType type)
	{
		URL = url;
		Type = type;
	}

	public ServerAddress(string scheme, string host, int port, ServerAddressType type)
	{
		URL = new UriBuilder
		{
			Scheme = scheme,
			Host = host,
			Port = port
		}.Uri.ToString();
		Type = type;
	}

	public ServerAddress(string url)
		: this(url, ServerAddressType.Internal)
	{
	}

	public ServerAddress()
		: this(null, ServerAddressType.Internal)
	{
	}

	public static bool ValidateUrl(string url, bool allowHttps)
	{
		if (Uri.TryCreate(url, UriKind.Absolute, out var result))
		{
			if (string.Equals(result.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) || (string.Equals(result.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) && allowHttps))
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
