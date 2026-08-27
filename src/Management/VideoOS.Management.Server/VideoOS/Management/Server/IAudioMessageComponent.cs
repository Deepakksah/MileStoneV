using System;

namespace VideoOS.Management.Server;

public interface IAudioMessageComponent<T> : IDisposable
{
	void CreateAudioMessage(string id, string name, string description, string originalFileName, byte[] content, int maxMessageCount);

	string[] QueryAudioMessageIds();

	T[] QueryAudioMessages();

	T[] QueryAudioMessagesByIds(string[] ids);

	T QueryAudioMessageById(string id);

	byte[] QueryAudioMessageContentById(string id);

	void UpdateAudioMessage(string id, string name, string description);

	void UpdateAudioMessageContent(string id, string originalFileName, byte[] content, Guid sessionId);

	void DeleteAudioMessage(string id, Guid sessionId);
}
