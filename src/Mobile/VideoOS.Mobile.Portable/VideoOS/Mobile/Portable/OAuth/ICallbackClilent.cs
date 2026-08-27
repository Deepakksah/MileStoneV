using System;
using System.Net;

namespace VideoOS.Mobile.Portable.OAuth;

public interface ICallbackClilent
{
	Uri RedirectUrl { get; }

	void SubscribeForCode(Action<HttpListenerContext> subscriber);
}
