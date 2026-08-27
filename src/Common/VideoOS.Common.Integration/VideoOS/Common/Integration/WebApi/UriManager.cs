using System;

namespace VideoOS.Common.Integration.WebApi;

public class UriManager
{
	private readonly string _schemeToUse;

	public UriManager(bool useSecureScheme)
	{
		_schemeToUse = (useSecureScheme ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
	}

	public Uri FormAbsolute(string host, int port)
	{
		return FormAbsolute(host, port, null);
	}

	public Uri FormAbsolute(string host, int port, string path)
	{
		return new UriBuilder(_schemeToUse, host, port, path).Uri;
	}

	public string FormWithWildcard(int port)
	{
		return FormattableString.Invariant($"{_schemeToUse}{Uri.SchemeDelimiter}+:{port}/");
	}
}
