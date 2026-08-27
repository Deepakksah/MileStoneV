using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace VideoOS.Common.Integration.Communication;

public static class UriHandling
{
	private const string NoEndpointsAvailableMessage = "The URI must have either a secure or unsecure endpoint available";

	private const string UnsecureNotAllowedMessage = "The URI only has an unsecure endpoint, but unsecure is not allowed";

	private const int DefaultTimeout = 60000;

	public static Uri CreateHttpUri(string url, bool useDefaultPort)
	{
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentNullException("url");
		}
		return CreateUriWithScheme(url, Uri.UriSchemeHttp, useDefaultPort);
	}

	public static Uri CreateHttpUri(string url, int port)
	{
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentNullException("url");
		}
		ValidatePort(port);
		return CreateUriWithSchemeAndPort(url, Uri.UriSchemeHttp, port);
	}

	public static Uri CreateHttpsUri(string url, bool useDefaultPort)
	{
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentNullException("url");
		}
		return CreateUriWithScheme(url, Uri.UriSchemeHttps, useDefaultPort);
	}

	public static Uri CreateHttpsUri(string url, int port)
	{
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentNullException("url");
		}
		ValidatePort(port);
		return CreateUriWithSchemeAndPort(url, Uri.UriSchemeHttps, port);
	}

	public static int ConvertStringToPort(string value)
	{
		int num = int.Parse(value, CultureInfo.InvariantCulture);
		ValidatePort(num);
		return num;
	}

	public static Uri GetSecureUri(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		uri = EnsureUriHasValidScheme(uri);
		return new UriBuilder(uri)
		{
			Scheme = Uri.UriSchemeHttps,
			Port = (IsStandardPort(uri.Port) ? 443 : uri.Port)
		}.Uri;
	}

	public static Uri GetUnsecureUri(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		uri = EnsureUriHasValidScheme(uri);
		return new UriBuilder(uri)
		{
			Scheme = Uri.UriSchemeHttp,
			Port = (IsStandardPort(uri.Port) ? 80 : uri.Port)
		}.Uri;
	}

	public static Uri EnsureUriHasValidScheme(Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (UriHasValidScheme(uri))
		{
			if (IsStandardScheme(uri.Scheme))
			{
				return uri;
			}
			throw new InvalidOperationException("Only URIs with a scheme of " + Uri.UriSchemeHttps + " or " + Uri.UriSchemeHttp + " are allowed");
		}
		return AddValidScheme(uri);
	}

	private static Uri AddValidScheme(Uri uri)
	{
		uri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", Uri.UriSchemeHttps, uri));
		if (uri.Port == 80)
		{
			return new UriBuilder(uri)
			{
				Scheme = Uri.UriSchemeHttp
			}.Uri;
		}
		return uri;
	}

	private static Uri CreateUriWithScheme(string url, string scheme, bool useDefaultPort)
	{
		bool? flag = DoesUrlNeedModification(url);
		if (flag.HasValue && flag.Value)
		{
			url = ModifyUrlToIPv6(url).ToString();
		}
		Uri uri = new UriBuilder(url).Uri;
		if (UriHasValidScheme(uri))
		{
			return new UriBuilder(uri)
			{
				Scheme = scheme,
				Port = ((useDefaultPort || uri.IsDefaultPort) ? (-1) : uri.Port)
			}.Uri;
		}
		uri = new UriBuilder(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", scheme, uri)).Uri;
		return new UriBuilder(uri)
		{
			Port = ((useDefaultPort || uri.IsDefaultPort) ? (-1) : uri.Port)
		}.Uri;
	}

	private static StringBuilder ModifyUrlToIPv6(string currentUrl)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[").Append(currentUrl).Append("]");
		return stringBuilder;
	}

	private static bool? DoesUrlNeedModification(string url)
	{
		IPAddress address = null;
		if (IPAddress.TryParse(url, out address))
		{
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				return false;
			}
			if (address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				return true;
			}
		}
		return null;
	}

	private static Uri CreateUriWithSchemeAndPort(string url, string scheme, int port)
	{
		Uri uri = new UriBuilder(url).Uri;
		if (UriHasValidScheme(uri))
		{
			return new UriBuilder(uri)
			{
				Scheme = scheme,
				Port = port
			}.Uri;
		}
		return new UriBuilder(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", scheme, uri))
		{
			Port = port
		}.Uri;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsStandardPort(int port)
	{
		if (port != 443)
		{
			return port == 80;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsStandardScheme(string scheme)
	{
		if (!(scheme.ToLowerInvariant() == Uri.UriSchemeHttps))
		{
			return scheme.ToLowerInvariant() == Uri.UriSchemeHttp;
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ValidatePort(int port)
	{
		if (port < 0 || port > 65535)
		{
			throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "Invalid port number '{0}': The valid range of ports is from 0 to 65535", port));
		}
	}

	private static bool UriHasValidScheme(Uri uri)
	{
		if (!(uri.Scheme == Uri.UriSchemeHttp) && !(uri.Scheme == Uri.UriSchemeHttps) && !(uri.Scheme == Uri.UriSchemeFile) && !(uri.Scheme == Uri.UriSchemeFtp) && !(uri.Scheme == Uri.UriSchemeGopher) && !(uri.Scheme == Uri.UriSchemeMailto) && !(uri.Scheme == Uri.UriSchemeNetPipe) && !(uri.Scheme == Uri.UriSchemeNetTcp) && !(uri.Scheme == Uri.UriSchemeNews))
		{
			return uri.Scheme == Uri.UriSchemeNntp;
		}
		return true;
	}
}
