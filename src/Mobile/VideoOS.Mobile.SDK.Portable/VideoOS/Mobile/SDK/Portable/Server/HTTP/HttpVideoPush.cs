using VideoOS.Mobile.SDK.Portable.Server.Base;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpVideoPush<T> : HttpVideoPushBase<T> where T : PushStreamData, new()
{
	public HttpVideoPush(RequestStreamResponseUpload response, string extension, IConnectionEndPoint endPoint)
		: base(response, extension, endPoint, "http://")
	{
	}
}
