namespace VideoOS.Common.Integration;

public interface IThreadPoolMessageProcessor<T>
{
	int MaxProcessingMessages { get; set; }

	int PendingItemsCount { get; }

	int ProcessingItemsCount { get; }

	void EnableProcessing(bool enable);

	void QueueMessage(T message);

	void QueueMessages(T[] messages);

	void Start(bool enable);

	void Stop();
}
