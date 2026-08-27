using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace VideoOS.Utilities.Windows.Http;

public class UrlPrefix
{
	public const string HostStrongWildcard = "+";

	public const string HostWeakWildcard = "*";

	public UrlScheme Scheme { get; }

	public UrlReservationCategory Category { get; }

	public ushort Port { get; }

	public string Host { get; }

	public string RelativePath { get; }

	public override string ToString()
	{
		string text = null;
		text = Scheme switch
		{
			UrlScheme.Http => "http", 
			UrlScheme.Https => "https", 
			_ => throw new NotImplementedException("Unsupported scheme"), 
		};
		string text2 = null;
		switch (Category)
		{
		case UrlReservationCategory.StrongWildcard:
			text2 = "+";
			break;
		case UrlReservationCategory.WeakWildcard:
			text2 = "*";
			break;
		case UrlReservationCategory.Explicit:
		case UrlReservationCategory.IPBoundWeakWildcard:
			text2 = Host;
			break;
		default:
			throw new NotImplementedException("Unsupported category...");
		}
		if (string.IsNullOrEmpty(RelativePath))
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/", text, text2, Port);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}/", text, text2, Port, RelativePath);
	}

	public static UrlPrefix Parse(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("Invalid URL prefix format.", "value");
		}
		UrlScheme scheme;
		int num;
		if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
		{
			scheme = UrlScheme.Http;
			num = 7;
		}
		else
		{
			if (!value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException("Invalid URL prefix format. Unable to parse scheme.", "value");
			}
			scheme = UrlScheme.Https;
			num = 8;
		}
		int num2 = value.IndexOf('/', num);
		if (num2 == -1)
		{
			num2 = value.Length;
		}
		string relativePath = string.Empty;
		int num3 = num2 + 1;
		if (num3 < value.Length)
		{
			relativePath = value.Substring(num3, value.Length - num3);
		}
		int num4 = num2 - num;
		if (num4 < 3)
		{
			throw new ArgumentException("Invalid URL prefix format. Unable to parse host and port.", "value");
		}
		string text = value.Substring(num, num4);
		int num5 = text.LastIndexOf(':');
		int num6 = num5 + 1;
		if (num5 == -1)
		{
			throw new ArgumentException("Invalid URL prefix format. Unable extract port information.", "value");
		}
		string text2 = text.Substring(0, num5);
		if (!ushort.TryParse(text.Substring(num6, text.Length - num6), out var result) || result == 0)
		{
			throw new ArgumentException("Invalid URL prefix format. Incorrect port value.", "value");
		}
		if (string.IsNullOrEmpty(text2))
		{
			throw new ArgumentException("Invalid URL prefix format. Host name missing.", "value");
		}
		UrlReservationCategory categoryForHost = GetCategoryForHost(text2);
		return new UrlPrefix(scheme, categoryForHost, text2, result, relativePath);
	}

	private static UrlReservationCategory GetCategoryForHost(string hostString)
	{
		if (IPAddress.TryParse(hostString, out var _))
		{
			return UrlReservationCategory.IPBoundWeakWildcard;
		}
		if (hostString == "+")
		{
			return UrlReservationCategory.StrongWildcard;
		}
		if (hostString == "*")
		{
			return UrlReservationCategory.WeakWildcard;
		}
		return UrlReservationCategory.Explicit;
	}

	public static UrlPrefix CreateStrongWildcard(UrlScheme scheme, ushort port, string relativePath)
	{
		return new UrlPrefix(scheme, UrlReservationCategory.StrongWildcard, "+", port, relativePath);
	}

	public static UrlPrefix CreateExplicit(UrlScheme scheme, string host, ushort port, string relativePath)
	{
		if (string.IsNullOrEmpty(host))
		{
			throw new ArgumentException("Incorrect host value.", "host");
		}
		UrlReservationCategory categoryForHost = GetCategoryForHost(host);
		if (categoryForHost != UrlReservationCategory.Explicit)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Incorrect host value. Host matches special category {0}", categoryForHost), "host");
		}
		return new UrlPrefix(scheme, UrlReservationCategory.Explicit, host, port, relativePath);
	}

	public static UrlPrefix CreateIPBoundWeakWildcard(UrlScheme scheme, IPAddress host, ushort port, string relativePath)
	{
		if (host == null)
		{
			throw new ArgumentNullException("host");
		}
		string text = host.ToString();
		if (host.AddressFamily == AddressFamily.InterNetworkV6)
		{
			text = string.Format(CultureInfo.InvariantCulture, "[{0}]", text);
		}
		return new UrlPrefix(scheme, UrlReservationCategory.IPBoundWeakWildcard, text, port, relativePath);
	}

	public static UrlPrefix CreateWeakWildcard(UrlScheme scheme, ushort port, string relativePath)
	{
		return new UrlPrefix(scheme, UrlReservationCategory.WeakWildcard, "*", port, relativePath);
	}

	private UrlPrefix(UrlScheme scheme, UrlReservationCategory category, string host, ushort port, string relativePath)
	{
		if (port == 0)
		{
			throw new ArgumentOutOfRangeException("port", "Invalid port value.");
		}
		Scheme = scheme;
		Category = category;
		Host = host;
		Port = port;
		RelativePath = (relativePath ?? string.Empty).Trim('/');
	}
}
