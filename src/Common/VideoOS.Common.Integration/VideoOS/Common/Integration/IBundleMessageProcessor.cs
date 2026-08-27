namespace VideoOS.Common.Integration;

public interface IBundleMessageProcessor<T>
{
	int MaxMessagesPerCall { get; set; }

	int MillisecondsBetweenCalls { get; set; }

	void EnableProcessing(bool enable);

	void QueueMessage(T message);

	void QueueMessages(T[] messages);

	void Start(bool enable);

	void Stop();
}
