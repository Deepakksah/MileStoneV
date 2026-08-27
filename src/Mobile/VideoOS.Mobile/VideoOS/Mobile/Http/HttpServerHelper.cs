using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Net;

namespace VideoOS.Mobile.Http;

public static class HttpServerHelper
{
	public static HttpStatuses CorrectInputAddress(HttpListenerContext httpContext)
	{
		HttpListenerRequest request = httpContext.Request;
		if (!string.Equals(request.RawUrl, request.Url.PathAndQuery, StringComparison.InvariantCultureIgnoreCase))
		{
			return RedirectToAddress(httpContext, request.Url.PathAndQuery);
		}
		return HttpStatuses.Ok;
	}

	public static string ExtractRelativePath(string requestAddress, string listenPath)
	{
		requestAddress = HttpUtility.UrlDecode(requestAddress);
		return requestAddress.Substring(requestAddress.IndexOf(listenPath, StringComparison.CurrentCultureIgnoreCase) + listenPath.Count());
	}

	public static HttpStatuses RedirectToAddress(HttpListenerContext httpContext, string address)
	{
		httpContext.Response.AddHeaderExt("Location", address);
		return HttpStatuses.MovedPermanently;
	}

	public static List<string> SplitRelativePath(string relativePath)
	{
		return new List<string>(relativePath.Split(new char[1] { '/' }, 10, StringSplitOptions.RemoveEmptyEntries));
	}
}
