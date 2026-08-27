using System.Net;
using System.Text;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Utilities.JsonS;

namespace VideoOS.Mobile.Http;

public class HttpJsonHandler<T>
{
	private readonly JsonSerializer _serializer = new JsonSerializer(typeof(T));

	public HttpStatuses Process(HttpListenerContext httpContext, T objectToSerialize)
	{
		return HttpJsBaseHandler.Process(httpContext, delegate(StringBuilder builder)
		{
			_serializer.Serialize(builder, objectToSerialize);
		});
	}

	public HttpStatuses ProcessSimpleJson(HttpListenerContext httpContext, T objectToSerialize)
	{
		return HttpSimpleJsonHandler.Process(httpContext, delegate(StringBuilder builder)
		{
			_serializer.Serialize(builder, objectToSerialize);
		});
	}
}
