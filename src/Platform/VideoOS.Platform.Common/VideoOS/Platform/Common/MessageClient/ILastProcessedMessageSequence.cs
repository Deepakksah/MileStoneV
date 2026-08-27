namespace VideoOS.Platform.Common.MessageClient;

public interface ILastProcessedMessageSequence
{
	void Initialize();

	int GetLastProcessedMessageSequence();

	void UpdateLastProcessedMessageSequence(int newSequence);
}
