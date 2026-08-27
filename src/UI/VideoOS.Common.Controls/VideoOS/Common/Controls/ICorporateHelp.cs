namespace VideoOS.Common.Controls;

public interface ICorporateHelp
{
	void ShowHelpTopic(string internalTopicId);

	void ShowHelpTopic(string internalTopicId, string defaultFallbackTopicId);

	void ShowContentHelp();

	void ShowFindHelp();
}
