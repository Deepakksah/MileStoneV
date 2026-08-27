using System;

namespace VideoOS.Platform.Util;

public static class UrlUtil
{
	public static Uri BuildUri(string url)
	{
		try
		{
			Uri uri = new UriBuilder(url).Uri;
			if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeFile)
			{
				uri = new UriBuilder("http://" + url).Uri;
			}
			return uri;
		}
		catch (Exception)
		{
			EnvironmentManager.Instance.Log(error: true, "UrlUtil.BuildUrl", "Unable to build url from:" + url);
			throw new MIPException("Unable to build Url - check log");
		}
	}
}
