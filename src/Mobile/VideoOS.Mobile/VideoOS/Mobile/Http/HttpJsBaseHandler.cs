using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Http;

public class HttpJsBaseHandler
{
	public delegate void WriteJsonData(StringBuilder builder);

	public const string CallbackNameDefault = "callback";

	private const string RegExIdentifier = "[^\\w\\.]";

	private static readonly Regex RegExChecker = new Regex("[^\\w\\.]");

	public static HttpStatuses Process(HttpListenerContext httpContext, WriteJsonData jsonWriter)
	{
		HttpStatuses result = HttpStatuses.Ok;
		HttpListenerRequest request = httpContext.Request;
		HttpListenerResponse response = httpContext.Response;
		try
		{
			string text = "callback";
			if (request.QueryString.AllKeys.Contains(text))
			{
				text = request.QueryString[text];
			}
			if (!IsValidIdentifier(text))
			{
				text = "callback";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(text).Append("(");
			StringBuilder stringBuilder2 = new StringBuilder();
			jsonWriter(stringBuilder2);
			JsonConvert.DeserializeObject(stringBuilder2.ToString());
			stringBuilder.Append(stringBuilder2);
			stringBuilder.Append(")");
			byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			response.ContentEncoding = Encoding.UTF8;
			response.ContentLength64 = bytes.Length;
			response.ContentType = "application/javascript";
			response.OutputStream.Write(bytes, 0, bytes.Length);
		}
		catch (Exception ex)
		{
			result = HttpStatuses.InternalServerError;
			Log.Error("HttpJsBaseHandler.Process", ex.Message);
		}
		return result;
	}

	public static bool IsValidIdentifier(string name)
	{
		return !RegExChecker.IsMatch(name);
	}
}
