using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class StateEventArgs<T> : EventArgs
{
	public T State { get; private set; }

	public StateEventArgs(T state)
	{
		State = state;
	}
}
