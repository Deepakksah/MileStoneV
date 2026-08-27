using System;

namespace VideoOS.IdentityServer.Common.Utilities.ServerValidation;

public static class UriExtender
{
	private const string SchemeFile = "file";

	private const string SchemeFtp = "ftp";

	private const string SchemeGopher = "gopher";

	private const string SchemeHttp = "http";

	private const string SchemeHttps = "https";

	private const string SchemeMailTo = "mailto";

	private const string SchemeNetPipe = "net.pipe";

	private const string SchemeNetTcp = "net.tcp";

	private const string SchemeNews = "news";

	private const string SchemeNntp = "nntp";

	public static bool HasValidScheme(this Uri uri)
	{
		if (uri == null)
		{
			return false;
		}
		switch (uri.Scheme)
		{
		case "nntp":
		case "http":
		case "news":
		case "file":
		case "gopher":
		case "mailto":
		case "ftp":
		case "https":
		case "net.pipe":
		case "net.tcp":
			return true;
		default:
			return false;
		}
	}
}
