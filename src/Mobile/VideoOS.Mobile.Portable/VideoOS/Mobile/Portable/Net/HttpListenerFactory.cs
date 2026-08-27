using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Net;

public class HttpListenerFactory<T> : IHttpListenerFactory where T : IHttpListener, IHttpListenerConfig, new()
{
	public IHttpListener CreateListener(IEnumerable<string> prefixes)
	{
		T val = new T();
		val.SetPrefixes(prefixes);
		return val;
	}
}
