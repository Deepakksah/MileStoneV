using Microsoft.Extensions.Logging;

namespace VideoOS.Common.Help.UI;

public interface IHelpManager
{
	bool IsOnline { get; }

	string UrlTemplate { get; }

	string OfflineUrlTemplate { get; }

	string Version { get; }

	string CurrentLocaleId { get; }

	string OriginalLocaleId { get; }

	ILogger Logger { get; }

	bool Initialize(string url, string offlineUrl, string version, string localeId, bool workOffline);

	bool Initialize(string url, string offlineUrl, string version, string localeId, bool workOffline, ILogger logger);

	bool ShowHelp(int topicId);

	void Close();
}
