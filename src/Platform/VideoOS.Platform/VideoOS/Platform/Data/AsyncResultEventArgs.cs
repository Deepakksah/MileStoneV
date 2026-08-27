using System;

namespace VideoOS.Platform.Data;

public class AsyncResultEventArgs : EventArgs
{
	public object Result;

	public object AsyncState;
}
