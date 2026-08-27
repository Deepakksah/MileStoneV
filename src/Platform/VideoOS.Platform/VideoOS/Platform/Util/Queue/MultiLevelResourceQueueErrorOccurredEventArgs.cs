using System;

namespace VideoOS.Platform.Util.Queue;

public class MultiLevelResourceQueueErrorOccurredEventArgs<TOutput> : EventArgs
{
	public MultiLevelResourceQueueInput<TOutput> Input { get; }

	public string ErrorMessage { get; }

	public Exception Exception { get; }

	public MultiLevelResourceQueueErrorOccurredEventArgs(MultiLevelResourceQueueInput<TOutput> input, string errorMessage, Exception exception)
	{
		Input = input;
		ErrorMessage = errorMessage;
		Exception = exception;
	}
}
