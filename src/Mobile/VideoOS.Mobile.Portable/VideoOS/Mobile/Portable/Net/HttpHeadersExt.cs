using System.Net;

namespace VideoOS.Mobile.Portable.Net;

public static class HttpHeadersExt
{
	public static void AddHeaderExt(this HttpListenerResponse response, string key, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			response.AddHeader(key, value);
		}
	}

	public static void RemoveHeaderExt(this HttpListenerResponse response, string key)
	{
		if (!string.IsNullOrWhiteSpace(key))
		{
			response.AddHeader(key, null);
		}
	}
}
