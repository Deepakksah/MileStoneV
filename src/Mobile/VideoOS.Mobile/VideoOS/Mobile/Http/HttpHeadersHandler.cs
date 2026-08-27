using System;
using System.Net;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Http;

public class HttpHeadersHandler
{
	private static readonly HttpDictinaryHandler Handler = new HttpDictinaryHandler();

	public static HttpStatuses Process(HttpListenerContext context)
	{
		try
		{
			HttpListenerRequest request = context.Request;
			return Handler.Process(context, request.Headers);
		}
		catch (Exception ex)
		{
			Log.Error("HttpHeadersHandler.Process", ex.Message);
			return HttpStatuses.InternalServerError;
		}
	}
}
