using System;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class ResponseException : WebApiException
{
	public HttpStatusCode StatusCode { get; set; }

	public JObject Content { get; set; }

	public ResponseException()
	{
	}

	public ResponseException(string message)
		: base(message)
	{
	}

	public ResponseException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ResponseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		StatusCode = (HttpStatusCode)info.GetValue("StatusCode", typeof(HttpStatusCode));
		if (info.GetValue("Content", typeof(string)) is string text)
		{
			Content = JObject.Parse(text);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("StatusCode", StatusCode);
		info.AddValue("Content", ((object)Content)?.ToString());
		base.GetObjectData(info, context);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "StatusCode: {0} {1} {2}", StatusCode, Environment.NewLine, base.ToString());
	}
}
