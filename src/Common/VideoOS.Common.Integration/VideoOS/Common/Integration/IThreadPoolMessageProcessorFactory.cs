namespace VideoOS.Common.Integration;

public interface IThreadPoolMessageProcessorFactory<T>
{
	IThreadPoolMessageProcessor<T> Create(ThreadPoolMessageProcessor<T>.ProcessDelegate processDelegate, IEventTrace eventTraceObj);
}
