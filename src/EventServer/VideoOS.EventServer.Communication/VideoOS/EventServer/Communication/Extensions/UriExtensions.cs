using System;

namespace VideoOS.EventServer.Communication.Extensions;

public static class UriExtensions
{
	public static bool IsHttps([ValidatedNotNull] this Uri uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		return uri.Scheme == Uri.UriSchemeHttps;
	}
}
