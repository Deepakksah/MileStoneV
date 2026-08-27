using System;
using System.Collections.Specialized;
using System.Web;

namespace VideoOS.Platform.Common.MessageClient;

public static class UriExtensions
{
	public static Uri AddParameter(this Uri url, string paramName, string paramValue)
	{
		UriBuilder uriBuilder = new UriBuilder(url);
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
		nameValueCollection[paramName] = paramValue;
		uriBuilder.Query = nameValueCollection.ToString() ?? string.Empty;
		return uriBuilder.Uri;
	}
}
