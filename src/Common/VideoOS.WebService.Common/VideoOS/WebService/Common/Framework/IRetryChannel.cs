using System;

namespace VideoOS.WebService.Common.Framework;

public interface IRetryChannel<out TChannel> where TChannel : class
{
	void CallWithRetry(Action<TChannel> call);

	TResult CallWithRetry<TResult>(Func<TChannel, TResult> call);

	void CallWithRetry(Action<TChannel> call, Func<Exception, bool> manualErrorHandler);

	TResult CallWithRetry<TResult>(Func<TChannel, TResult> call, Func<Exception, bool> manualErrorHandler);
}
