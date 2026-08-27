using System;
using System.Threading;

namespace VideoOS.Common.Integration.Threading;

public interface IWorkItemResult : IWorkItemResult<object>, IWaitableResult
{
}
public interface IWorkItemResult<TResult> : IWaitableResult
{
	bool IsCompleted { get; }

	bool IsCanceled { get; }

	object State { get; }

	WorkItemPriority WorkItemPriority { get; }

	TResult Result { get; }

	object Exception { get; }

	TResult GetResult();

	TResult GetResult(int millisecondsTimeout, bool exitContext);

	TResult GetResult(TimeSpan timeout, bool exitContext);

	TResult GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle);

	TResult GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle);

	TResult GetResult(out Exception exception);

	TResult GetResult(int millisecondsTimeout, bool exitContext, out Exception exception);

	TResult GetResult(TimeSpan timeout, bool exitContext, out Exception exception);

	TResult GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception exception);

	TResult GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception exception);

	bool Cancel();

	bool Cancel(bool abortExecution);
}
