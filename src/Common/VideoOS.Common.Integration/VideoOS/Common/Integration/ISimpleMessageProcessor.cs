namespace VideoOS.Common.Integration;

public interface ISimpleMessageProcessor<T>
{
	void EnableProcessing(bool enable);

	void QueueMessage(T message);

	void QueueMessages(T[] messages);

	void Start(bool enable);

	void Stop();
}
