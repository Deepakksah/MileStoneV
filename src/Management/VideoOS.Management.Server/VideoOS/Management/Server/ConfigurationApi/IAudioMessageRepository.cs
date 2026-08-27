namespace VideoOS.Management.Server.ConfigurationApi;

public interface IAudioMessageRepository
{
	AudioMessage QueryAudioMessageById(IGeminiRequestContext requestContext, string id);

	AudioMessage[] QueryAudioMessages(IGeminiRequestContext requestContext);

	AudioMessage UpdateAudioMessage(IGeminiRequestContext requestContext, string id, string name, string description);

	void UpdateAudioMessageValidation(IGeminiRequestContext requestContext, string id, string name, string description);

	AudioMessage CreateAudioMessage(IGeminiRequestContext requestContext, string id, string name, string description, string originalFileName, string base64Content);

	void DeleteAudioMessage(IGeminiRequestContext requestContext, string id);

	string GetMessageContent(IGeminiRequestContext requestContext, string id);

	void SetMessageContent(IGeminiRequestContext requestContext, string id, string originalFileName, string base64Content);
}
