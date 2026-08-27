namespace VideoOS.Platform.Util.Queue;

public interface IMultiLevelResourceQueueDiagnostics
{
	int MaxConcurrentRequests { get; }

	int MaxIdleTimeMillisBeforeResourceProcessorCleanup { get; }

	int ConcurrentRequests { get; }

	int ActiveResourceProcessors { get; }

	int IdleResourceProcessors { get; }

	int ResourceProcessors { get; }

	int RequestsInQueue { get; }
}
