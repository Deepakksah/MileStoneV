using System;
using System.Net;
using System.Text;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Http;

public class HttpSimpleJsonHandler
{
	public delegate void WriteJsonData(StringBuilder builder);

	public static HttpStatuses Process(HttpListenerContext httpContext, WriteJsonData jsonWriter)
	{
		HttpStatuses result = HttpStatuses.Ok;
		HttpListenerResponse response = httpContext.Response;
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			jsonWriter(stringBuilder);
			byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			response.ContentEncoding = Encoding.UTF8;
			response.ContentLength64 = bytes.Length;
			response.ContentType = "text/json";
			response.OutputStream.Write(bytes, 0, bytes.Length);
		}
		catch (Exception ex)
		{
			result = HttpStatuses.InternalServerError;
			Log.Error("HttpSimpleJsonHandler.Process", ex.Message);
		}
		return result;
	}
}
