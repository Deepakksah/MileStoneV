using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Utilities.Validation;

namespace VideoOS.Common.Help.UI;

public class HelpManager : IHelpManager, IDisposable
{
	private bool _isInitialized;

	private bool _offLineHelpAvailable = true;

	private bool _goOnline = true;

	private const string _defaultLocaleId = "en-US";

	private object _lockIsOnline = new object();

	private readonly IHelpDisplayer _helpDisplayer;

	private readonly IHelpUriChecker _helpUriChecker;

	private bool _isHeartbeatOfflineChecked;

	private const string regValueIELockdown = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_LOCALMACHINE_LOCKDOWN";

	public ILogger Logger { get; private set; }

	public bool IsOnline { get; public set; }

	public string UrlTemplate { get; private set; }

	public string OfflineUrlTemplate { get; private set; }

	public string Version { get; private set; }

	public string CurrentLocaleId { get; private set; }

	public string OriginalLocaleId { get; private set; }

	public HelpManager(IHelpDisplayer helpDisplayer, IHelpUriChecker helpUriChecker)
	{
		_helpDisplayer = helpDisplayer;
		_helpUriChecker = helpUriChecker;
	}

	public bool Initialize(string url, string offlineUrl, string version, string localeId, bool workOffline)
	{
		return Initialize(url, offlineUrl, version, localeId, workOffline, null);
	}

	public bool Initialize(string url, string offlineUrl, string version, string localeId, bool workOffline, ILogger logger)
	{
		if (_isInitialized)
		{
			throw new HelpManagerAlreadyInitializedException();
		}
		url.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.Common.Help.UI\\prod\\VideoOS.Common.Help.UI\\VideoOS.Common.Help.UI\\HelpManager.cs", 83);
		offlineUrl.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.Common.Help.UI\\prod\\VideoOS.Common.Help.UI\\VideoOS.Common.Help.UI\\HelpManager.cs", 84);
		version.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.Common.Help.UI\\prod\\VideoOS.Common.Help.UI\\VideoOS.Common.Help.UI\\HelpManager.cs", 85);
		localeId.NullCheck("", "Initialize", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.Common.Help.UI\\prod\\VideoOS.Common.Help.UI\\VideoOS.Common.Help.UI\\HelpManager.cs", 86);
		Version = version;
		OriginalLocaleId = localeId;
		CurrentLocaleId = localeId;
		UrlTemplate = url;
		OfflineUrlTemplate = offlineUrl;
		Logger = logger;
		lock (_lockIsOnline)
		{
			IsOnline = false;
		}
		_goOnline = !workOffline;
		_isHeartbeatOfflineChecked = false;
		HeartbeatCheckAsync();
		_isInitialized = true;
		return true;
	}

	public bool ShowHelp(int topicId)
	{
		bool result = true;
		if (!_isInitialized)
		{
			throw new InvalidOperationException("Instance is not initialized. Call Initialize(..) first.");
		}
		if (topicId < 0)
		{
			topicId = 0;
			result = false;
		}
		Uri onlineUrl = new Uri(GetNavigationUrl(isOnlineUrl: true, UrlTemplate, OriginalLocaleId, topicId));
		Uri url = new Uri(GetNavigationUrl(IsOnline, IsOnline ? UrlTemplate : OfflineUrlTemplate, IsOnline ? OriginalLocaleId : CurrentLocaleId, topicId));
		EnsureHtmlActiveContentEnabled();
		DisplayHtmlHelp(url, onlineUrl);
		return result;
	}

	private string GetNavigationUrl(bool isOnlineUrl, string templateUri, string localeId, int topicId)
	{
		string text = ReplaceUrlVariables(templateUri, Version, localeId, topicId);
		if (!isOnlineUrl)
		{
			try
			{
				return new Uri(text).ToString();
			}
			catch (UriFormatException)
			{
			}
			text = ToAbsolutePathUrl(text);
			if (!text.StartsWith("file:///", StringComparison.OrdinalIgnoreCase))
			{
				text = "file:///" + text;
			}
		}
		return text;
	}

	private void HeartbeatCheckAsync()
	{
		Action[] array = new Action[2] { CheckOnlineHelp, CheckOfflineHelp };
		foreach (Action method in array)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, method);
		}
	}

	private void CheckOnlineHelp()
	{
		if (!_goOnline)
		{
			return;
		}
		string navigationUrl = GetNavigationUrl(isOnlineUrl: true, UrlTemplate, OriginalLocaleId, 0);
		bool isOnline = IsUrlAccessible(navigationUrl, logError: true);
		lock (_lockIsOnline)
		{
			IsOnline = isOnline;
		}
	}

	public void CheckOfflineHelp()
	{
		if (_isHeartbeatOfflineChecked)
		{
			return;
		}
		_isHeartbeatOfflineChecked = true;
		Uri uri = new Uri(GetNavigationUrl(isOnlineUrl: false, OfflineUrlTemplate, CurrentLocaleId, 0));
		if (uri.Scheme == Uri.UriSchemeFile)
		{
			if (_helpUriChecker.FileExists(uri))
			{
				return;
			}
			if (CurrentLocaleId == "en-US")
			{
				_offLineHelpAvailable = false;
				return;
			}
			Uri strOfflineUrl = new Uri(GetNavigationUrl(isOnlineUrl: false, OfflineUrlTemplate, "en-US", 0));
			if (!_helpUriChecker.FileExists(strOfflineUrl))
			{
				_offLineHelpAvailable = false;
			}
			else
			{
				CurrentLocaleId = "en-US";
			}
		}
		else if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
		{
			if (IsUrlAccessible(uri.ToString(), logError: true))
			{
				return;
			}
			if (CurrentLocaleId == "en-US")
			{
				_offLineHelpAvailable = false;
				return;
			}
			string strUrl = new Uri(GetNavigationUrl(isOnlineUrl: false, OfflineUrlTemplate, "en-US", 0)).ToString();
			if (!IsUrlAccessible(strUrl, logError: true))
			{
				_offLineHelpAvailable = false;
			}
			else
			{
				CurrentLocaleId = "en-US";
			}
		}
		else
		{
			ILogger logger = Logger;
			if (logger != null)
			{
				LoggerExtensions.LogInformation(logger, "HelpManager: Unsupported offline uri scheme: '{Schema}'", new object[1] { uri.Scheme });
			}
		}
	}

	private bool IsUrlAccessible(string strUrl, bool logError)
	{
		try
		{
			_helpUriChecker.CheckUriAccess(strUrl);
		}
		catch (Exception ex)
		{
			if (logError)
			{
				ILogger logger = Logger;
				if (logger != null)
				{
					LoggerExtensions.LogWarning(logger, "HelpManager: An error occurred while accessing help at url: '{Url}'. Error: {ExceptionMessage}", new object[2] { strUrl, ex.Message });
				}
			}
			return false;
		}
		return true;
	}

	[Conditional("DEBUG")]
	private static void _trace(string s)
	{
	}

	private static string ReplaceUrlVariables(string urlTemplate, string version, string localeId, int topicId)
	{
		return urlTemplate.Replace("{locale}", localeId).Replace("{version}", version).Replace("{topic}", topicId.ToString(CultureInfo.InvariantCulture));
	}

	public void Close()
	{
		if (_isInitialized)
		{
			_isInitialized = false;
		}
	}

	private static string ToAbsolutePathUrl(string actualUrl)
	{
		string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
		if (!IsAbsoluteUrl(actualUrl))
		{
			actualUrl = Path.Combine(path, actualUrl);
		}
		return actualUrl;
	}

	private void DisplayHtmlHelp(Uri url, Uri onlineUrl)
	{
		if (!IsOnline && !_offLineHelpAvailable)
		{
			throw new HelpManagerHelpNotAvailableException("HelpManager: An error occured while accessing help at url: " + url.AbsoluteUri)
			{
				OnlineUri = onlineUrl.OriginalString
			};
		}
		try
		{
			_helpDisplayer.DisplayHtmlHelp(IsOnline, url);
		}
		catch (Exception inner)
		{
			throw new HelpManagerHelpNotAvailableException("Cannot open help file.", inner)
			{
				OnlineUri = onlineUrl.OriginalString
			};
		}
	}

	private static void EnsureHtmlActiveContentEnabled()
	{
		try
		{
			if ((int)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_LOCALMACHINE_LOCKDOWN", "iexplore.exe", 1) != 0)
			{
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_LOCALMACHINE_LOCKDOWN", "iexplore.exe", 0, RegistryValueKind.DWord);
			}
		}
		catch
		{
		}
	}

	private static bool IsAbsoluteUrl(string url)
	{
		Uri result;
		return Uri.TryCreate(url, UriKind.Absolute, out result);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _isInitialized)
		{
			Close();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
