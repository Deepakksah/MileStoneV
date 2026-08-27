using System.Net;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public interface IHttpWebRequestFactory
{
	HttpWebRequest Create(string webAddress);
}
