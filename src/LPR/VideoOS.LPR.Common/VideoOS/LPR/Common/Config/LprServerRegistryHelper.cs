using System;
using System.IO;
using System.Net;
using System.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace VideoOS.LPR.Common.Config;

public class LprServerRegistryHelper : ILprServerRegistryHelper
{
	private Uri _serverUri;

	private static string _lprServerId;

	private string _installDir;

	private string _language;

	private readonly ILogger _log;

	private RegistryView? _currentRegistryView;

	private const string DefaultLanguage = "en-US";

	private const string RegistryPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Milestone\\XProtect LPR Server";

	private const string DefaultRegistryRelativePath = "SOFTWARE\\Milestone\\XProtect LPR Server";

	private const string ServerAddressRegistryKeyName = "VMSServerAddress";

	private const string InstallDirRegistryKeyName = "InstallDir";

	private const string ServiceInstanceRegistryKeyName = "ServiceInstance";

	private RegistryView CurrentRegistryView
	{
		get
		{
			if (!_currentRegistryView.HasValue)
			{
				using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
				using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Milestone\\XProtect LPR Server");
				_currentRegistryView = ((registryKey2 != null) ? RegistryView.Registry32 : RegistryView.Registry64);
			}
			return _currentRegistryView.Value;
		}
	}

	public Uri ServerUri
	{
		get
		{
			if (_serverUri != null)
			{
				return _serverUri;
			}
			string text = null;
			try
			{
				using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, CurrentRegistryView);
				_log.LogInformation(registryKey.ToString());
				using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Milestone\\XProtect LPR Server");
				_log.LogInformation(registryKey2.ToString());
				if (registryKey2 == null)
				{
					return null;
				}
				text = registryKey2.GetValue("VMSServerAddress") as string;
			}
			catch (Exception ex)
			{
				_log.LogError("ERROR: " + ex.Message);
			}
			finally
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					_log.LogInformation("Server Address was empty. Getting local machine address");
					text = new UriBuilder(Uri.UriSchemeHttp, Dns.GetHostEntry("").HostName, 80).Uri.AbsoluteUri;
				}
			}
			try
			{
				_serverUri = new Uri(text);
			}
			catch (UriFormatException ex2)
			{
				_log.LogWarning("Warning:\u00a0" + ex2.Message);
				_serverUri = new UriBuilder(Uri.UriSchemeHttp, text, 80).Uri;
			}
			return _serverUri;
		}
		set
		{
			if (!(value == null) && !(value == _serverUri))
			{
				_serverUri = value;
			}
		}
	}

	public Uri ServerHostUri => GetUriFromRegistryString(ServerUri);

	public string InstallDir
	{
		get
		{
			if (_installDir != null)
			{
				return _installDir;
			}
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, CurrentRegistryView);
			using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Milestone\\XProtect LPR Server");
			if (registryKey2 == null)
			{
				return null;
			}
			_installDir = registryKey2.GetValue("InstallDir") as string;
			return _installDir;
		}
	}

	public string ServerInstanceId
	{
		get
		{
			if (_lprServerId != null)
			{
				return _lprServerId;
			}
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, CurrentRegistryView);
			using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Milestone\\XProtect LPR Server");
			if (registryKey2 == null)
			{
				return null;
			}
			_lprServerId = registryKey2.GetValue("ServiceInstance") as string;
			return _lprServerId;
		}
		set
		{
			if (value == null)
			{
				return;
			}
			_lprServerId = value;
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			using RegistryKey registryKey2 = registryKey.CreateSubKey("SOFTWARE\\Milestone\\XProtect LPR Server");
			registryKey2?.SetValue("ServiceInstance", _lprServerId);
		}
	}

	public string Language
	{
		get
		{
			if (_language != null)
			{
				return _language;
			}
			try
			{
				_language = (Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Milestone\\XProtect LPR Server", "Language", "en-US") as string) ?? "en-US";
			}
			catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException || ex is IOException)
			{
				_log.LogError("Error when accessing language value in registry: " + ex.Message);
				_language = "en-US";
			}
			return _language;
		}
	}

	public LprServerRegistryHelper(ILogger log)
	{
		_log = log;
		if (string.IsNullOrEmpty(_lprServerId))
		{
			_lprServerId = ServerInstanceId;
		}
	}

	public bool SetServerUriInRegistry(Uri uri)
	{
		ServerUri = uri;
		try
		{
			Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Milestone\\XProtect LPR Server", "VMSServerAddress", _serverUri);
			return true;
		}
		catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException || ex is IOException)
		{
			_log.LogError("Error when saving server uri value in registry: " + ex.Message);
		}
		return false;
	}

	public void DeleteDefaultRegistryKey()
	{
		try
		{
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, CurrentRegistryView);
			registryKey.DeleteSubKey("SOFTWARE\\Milestone\\XProtect LPR Server");
		}
		catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException || ex is IOException)
		{
			_log.LogError("Error when deleting default key in registry: " + ex.Message);
		}
	}

	private Uri GetUriFromRegistryString(Uri uri)
	{
		if ((uri.HostNameType != UriHostNameType.Dns || uri.Host != "localhost") && (uri.HostNameType != UriHostNameType.IPv4 || uri.Host != "127.0.0.1") && (uri.HostNameType != UriHostNameType.IPv6 || uri.Host != "[::1]") && (uri.HostNameType != UriHostNameType.IPv6 || uri.Host != "[0000:0000:0000:0000:0000:0000:0000:0001]"))
		{
			return uri;
		}
		return new UriBuilder(uri)
		{
			Host = Dns.GetHostName()
		}.Uri;
	}

	public void CleanCache()
	{
		_currentRegistryView = null;
		_language = null;
		_serverUri = null;
		_installDir = null;
	}
}
