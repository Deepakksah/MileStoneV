using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Net;

public interface IHttpListener
{
	Dictionary<HttpMethods, OnHttpClient> SubscribedMethods { get; set; }

	bool TcpKeepAlive { get; set; }

	void SubsribeForAllMethods(OnHttpClient callback);

	void Start();

	void Stop();

	void Close();
}
