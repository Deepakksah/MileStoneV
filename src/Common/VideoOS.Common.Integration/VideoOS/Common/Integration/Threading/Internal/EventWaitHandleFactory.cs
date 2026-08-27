using System.Threading;

namespace VideoOS.Common.Integration.Threading.Internal;

public static class EventWaitHandleFactory
{
	public static AutoResetEvent CreateAutoResetEvent()
	{
		return new AutoResetEvent(initialState: false);
	}

	public static ManualResetEvent CreateManualResetEvent(bool initialState)
	{
		return new ManualResetEvent(initialState);
	}
}
