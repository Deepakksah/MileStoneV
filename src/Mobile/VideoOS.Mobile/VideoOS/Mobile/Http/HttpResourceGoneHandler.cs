using System.Net;
using VideoOS.Mobile.Portable.Http;

namespace VideoOS.Mobile.Http;

public class HttpResourceGoneHandler
{
	public static HttpFixedStatusCodeHandler Instance = new HttpFixedStatusCodeHandler(HttpStatuses.Gone);

	public static HttpStatuses Process(HttpListenerContext context)
	{
		return Instance.Process(context);
	}
}
