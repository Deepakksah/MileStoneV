using System;
using System.Net;
using System.Runtime.Serialization;

namespace VideoOS.Common.EnvironmentServices.Web;

[Serializable]
public class WebException : System.Net.WebException
{
	public new IWebResponse Response { get; }

	public WebException(System.Net.WebException nativeException)
		: this(nativeException.Message, nativeException.InnerException, nativeException.Status, nativeException.Response, WebRequest.WrapResponse(nativeException.Response))
	{
	}

	public WebException(string message, Exception inner, WebExceptionStatus status, System.Net.WebResponse nativeResponse, IWebResponse wrappedResponse)
		: base(message, inner, status, nativeResponse)
	{
		Response = wrappedResponse;
	}

	protected WebException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		Response = (WebResponse)info.GetValue("Response", typeof(WebResponse));
	}

	public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		base.GetObjectData(serializationInfo, streamingContext);
		serializationInfo.AddValue("Response", Response);
	}
}
