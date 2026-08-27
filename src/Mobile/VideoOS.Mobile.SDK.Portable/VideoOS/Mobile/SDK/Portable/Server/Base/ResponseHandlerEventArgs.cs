using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class ResponseHandlerEventArgs<T> : EventArgs
{
	public T Response { get; set; }
}
