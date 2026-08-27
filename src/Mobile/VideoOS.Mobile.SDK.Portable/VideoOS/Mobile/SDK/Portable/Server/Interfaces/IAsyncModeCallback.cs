using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IAsyncModeCallback
{
	void ProcessIAsyncResult(IAsyncResult result, AsyncCallback callback, TimeSpan timeout);
}
