using System;

namespace VideoOS.Common.Integration.Extensions;

public static class UriExtensions
{
	public static Uri SetScheme(this Uri uri, string scheme)
	{
		return new UriBuilder(uri)
		{
			Scheme = scheme
		}.Uri;
	}

	public static Uri SetPort(this Uri uri, int port)
	{
		return new UriBuilder(uri)
		{
			Port = port
		}.Uri;
	}
}
