namespace VideoOS.Common.Integration;

public class ThreadPoolMessageProcessorFactory<T> : IThreadPoolMessageProcessorFactory<T>
{
	public IThreadPoolMessageProcessor<T> Create(ThreadPoolMessageProcessor<T>.ProcessDelegate processDelegate, IEventTrace eventTraceObj)
	{
		return new ThreadPoolMessageProcessor<T>(processDelegate, eventTraceObj);
	}
}
