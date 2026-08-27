namespace VideoOS.Platform.Data;

public interface IThumbnailQueueDiagnostics
{
	int MaxConcurrentRequests { get; }

	int MaxIdleTimeMillisBeforeVideoSourceCleanup { get; }

	int ConcurrentRequests { get; }

	int ActiveVideoSources { get; }

	int IdleVideoSources { get; }

	int VideoSources { get; }

	int RequestsInQueue { get; }
}
