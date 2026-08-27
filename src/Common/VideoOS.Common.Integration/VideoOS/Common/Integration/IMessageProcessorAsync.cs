using System.Threading.Tasks;

namespace VideoOS.Common.Integration;

public interface IMessageProcessorAsync<T>
{
	int MaxProcessingMessages { get; set; }

	int PendingItemsCount { get; }

	int ProcessingItemsCount { get; }

	Task EnableProcessing(bool enable);

	Task QueueMessage(T message);

	Task QueueMessages(T[] messages);

	Task Start(bool enable);

	void Stop();
}
