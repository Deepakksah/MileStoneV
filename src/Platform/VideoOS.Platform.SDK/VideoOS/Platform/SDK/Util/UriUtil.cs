using System;

namespace VideoOS.Platform.SDK.Util;

public class UriUtil
{
	public static bool UriEqual(Uri uri1, Uri uri2, bool ignoreHttpHttpsDifference)
	{
		if (uri1 == null)
		{
			throw new ArgumentNullException("uri1");
		}
		if (uri2 == null)
		{
			throw new ArgumentNullException("uri2");
		}
		if (string.Compare(uri1.DnsSafeHost, uri2.DnsSafeHost, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return UriPortEqual(uri1, uri2, ignoreHttpHttpsDifference);
		}
		string[] array = uri1.DnsSafeHost.Split('.');
		string[] array2 = uri2.DnsSafeHost.Split('.');
		if (array.Length != array2.Length && string.Compare(array2[0], array[0], StringComparison.OrdinalIgnoreCase) == 0)
		{
			return UriPortEqual(uri1, uri2, ignoreHttpHttpsDifference);
		}
		return false;
	}

	public static bool UriPortEqual(Uri uri1, Uri uri2, bool ignoreHttpHttpsDifference)
	{
		if (ignoreHttpHttpsDifference && HasDefaultUriSchemeAndPort(uri1) && HasDefaultUriSchemeAndPort(uri2))
		{
			return true;
		}
		if (uri1.Port == uri2.Port)
		{
			return true;
		}
		if (uri1.Port == 80 && uri2.Port == 0)
		{
			return true;
		}
		if (uri1.Port == 0 && uri2.Port == 80)
		{
			return true;
		}
		if (uri1.Port == 443 && uri2.Port == 0 && uri2.Scheme == Uri.UriSchemeHttps)
		{
			return true;
		}
		if (uri2.Port == 443 && uri1.Port == 0 && uri1.Scheme == Uri.UriSchemeHttps)
		{
			return true;
		}
		return false;
	}

	private static bool HasDefaultUriSchemeAndPort(Uri uri)
	{
		if (uri.Port == 80 || uri.Port == 443)
		{
			if (!(uri.Scheme == Uri.UriSchemeHttps))
			{
				return uri.Scheme == Uri.UriSchemeHttp;
			}
			return true;
		}
		return false;
	}

	public static Uri ConstructLegacyServiceUri(Uri uri, bool isBasicUser, string servicePath)
	{
		int port = uri.Port;
		if (isBasicUser && uri.Port == 80)
		{
			port = 443;
		}
		else if (!isBasicUser && uri.Port == 443)
		{
			port = 80;
		}
		if (!isBasicUser)
		{
			return new UriBuilder("http", uri.DnsSafeHost)
			{
				Path = servicePath,
				Port = port
			}.Uri;
		}
		return new UriBuilder("https", uri.DnsSafeHost)
		{
			Path = servicePath,
			Port = port
		}.Uri;
	}

	public static string EnsureHttpPrefix(string address, string defaultScheme)
	{
		if (address.StartsWith("http"))
		{
			return address;
		}
		if (defaultScheme == Uri.UriSchemeHttps)
		{
			return "https://" + address;
		}
		return "http://" + address;
	}
}
