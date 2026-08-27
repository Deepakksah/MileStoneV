using System;

namespace VideoOS.Mobile.Utilities;

public static class UriStringHelper
{
	private static readonly string HttpPrefix = Uri.UriSchemeHttp + "://";

	private static readonly string HttpsPrefix = Uri.UriSchemeHttps + "://";

	public static string PrependSchemeIfMissing(string uri, string scheme)
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(uri))
		{
			text = uri;
		}
		if (!text.StartsWith(HttpPrefix, StringComparison.CurrentCultureIgnoreCase) && !text.StartsWith(HttpsPrefix, StringComparison.CurrentCultureIgnoreCase))
		{
			text = scheme + "://" + uri;
		}
		return text;
	}

	public static string PrependHttpSchemeIfMissing(string uri)
	{
		return PrependSchemeIfMissing(uri, Uri.UriSchemeHttp);
	}
}
