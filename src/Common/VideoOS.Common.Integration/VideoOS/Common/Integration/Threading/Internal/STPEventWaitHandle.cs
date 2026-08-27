using System.Threading;

namespace VideoOS.Common.Integration.Threading.Internal;

public static class STPEventWaitHandle
{
	public const int WaitTimeout = -1;

	public static bool WaitAll(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
	{
		return WaitHandle.WaitAll(waitHandles, millisecondsTimeout, exitContext);
	}

	public static int WaitAny(WaitHandle[] waitHandles)
	{
		return WaitHandle.WaitAny(waitHandles);
	}

	public static int WaitAny(WaitHandle[] waitHandles, int millisecondsTimeout, bool exitContext)
	{
		return WaitHandle.WaitAny(waitHandles, millisecondsTimeout, exitContext);
	}

	public static bool WaitOne(WaitHandle waitHandle, int millisecondsTimeout, bool exitContext)
	{
		return waitHandle.WaitOne(millisecondsTimeout, exitContext);
	}
}
