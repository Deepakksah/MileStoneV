namespace VideoOS.Common.Integration;

public interface IMessagePersister<T>
{
	bool HasMessages { get; }

	void Open();

	void Close();

	void PersistMessages(T[] messages);

	T[] GetRecentMessages(int maxCount);
}
