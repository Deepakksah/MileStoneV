namespace VideoOS.Common.Integration;

public class ThreadPoolBulkMessageProcessorFactory<T> : IThreadPoolMessageProcessorFactory<T>
{
	private int maxCommandsToBulk;

	public ThreadPoolBulkMessageProcessorFactory(int maxCommandsToBulk)
	{
		this.maxCommandsToBulk = maxCommandsToBulk;
	}

	public IThreadPoolMessageProcessor<T> Create(ThreadPoolMessageProcessor<T>.ProcessDelegate processDelegate, IEventTrace eventTraceObj)
	{
		return new ThreadPoolBulkMessageProcessor<T>(processDelegate, eventTraceObj, maxCommandsToBulk);
	}
}
