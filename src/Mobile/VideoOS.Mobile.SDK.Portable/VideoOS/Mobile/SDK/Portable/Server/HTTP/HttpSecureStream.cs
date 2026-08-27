using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpSecureStream : HttpStream
{
	public override bool IsSecured => true;

	public HttpSecureStream(IRequestStreamResponse response, string extension, IConnectionEndPoint endPoint)
		: base(response, extension, endPoint)
	{
	}
}
