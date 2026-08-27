using System;
using System.Configuration;
using System.IO;
using Microsoft.Win32;

namespace VideoOS.Reporting.Web.Plugin.Settings;

public class ConfigSettings
{
	private static string _serverInstallRegistryKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\VideoOS\\Installation\\Server";

	private static int _webSitePort = int.MinValue;

	private static string _serverLogFolder;

	public static int WebSitePort
	{
		get
		{
			if (_webSitePort == int.MinValue)
			{
				object value = Registry.GetValue(_serverInstallRegistryKey, "WebSitePort", string.Empty);
				if (value == null)
				{
					return 80;
				}
				if (int.TryParse(value.ToString(), out var result))
				{
					return result;
				}
				throw new InvalidOperationException("App setting '" + _serverInstallRegistryKey + "\\WebSitePort' is missing or empty.");
			}
			return _webSitePort;
		}
	}

	public static bool Secure => AuthorizationServerUri.Scheme == Uri.UriSchemeHttps;

	public static Uri AuthorizationServerUri
	{
		get
		{
			string text = ConfigurationManager.AppSettings.Get("AuthorizationServerUri");
			if (string.IsNullOrEmpty(text))
			{
				text = "http://localhost:80/IDP";
			}
			return new Uri(text);
		}
	}

	public static Guid ClientRegistrationId
	{
		get
		{
			if (Guid.TryParse(ConfigurationManager.AppSettings.Get("ClientRegistrationId"), out var result))
			{
				return result;
			}
			throw new InvalidOperationException("App setting 'ClientRegistrationId' is missing or could not be converted to an Int32.");
		}
	}

	public static int DataCollectorSampleFrequenceInSec
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.Reporting.Web.Plugin.DataCollector.SampleFrequenceInSec"], out var result))
			{
				return result;
			}
			throw new InvalidOperationException("App setting 'VideoOS.Reporting.Web.Plugin.DataCollector.SampleFrequenceInSec' is missing or could not be converted to an Int32.");
		}
	}

	public static int LiveDataCacheEntryLifetimeInSec
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.Reporting.Web.Plugin.LiveData.CacheEntryLifetimeInSec"], out var result))
			{
				return result;
			}
			throw new InvalidOperationException("App setting 'VideoOS.Reporting.Web.Plugin.LiveData.CacheEntryLifetimeInSec' is missing or could not be converted to an Int32.");
		}
	}

	public static int AuthorizationCacheEntryLifetimeInSec
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.Reporting.Web.Plugin.Authorization.CacheEntryLifetimeInSec"], out var result))
			{
				return result;
			}
			throw new InvalidOperationException("App setting 'VideoOS.Reporting.Web.Plugin.Authorization.CacheEntryLifetimeInSec' is missing or could not be converted to an Int32.");
		}
	}

	public static int AuthenticationCookieLifetimeInDays
	{
		get
		{
			if (int.TryParse(ConfigurationManager.AppSettings["VideoOS.Reporting.Web.Plugin.Security.AuthenticationCookieLifetimeInDays"], out var result))
			{
				return result;
			}
			throw new InvalidOperationException("App setting 'VideoOS.Reporting.Web.Plugin.Security.AuthenticationCookieLifetimeInDays' is missing or could not be converted to an Int32.");
		}
	}

	public static string ServerLogFolder
	{
		get
		{
			if (_serverLogFolder == null)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				OEMSettings oEMSettings = new OEMSettings();
				_serverLogFolder = Path.Combine(Path.Combine(folderPath, oEMSettings.CompanyName), oEMSettings.LoggingDir);
			}
			return _serverLogFolder;
		}
	}

	public static string DebugLogFileName
	{
		get
		{
			string text = ConfigurationManager.AppSettings["VideoOS.Reporting.Web.Plugin.DebugLogFileName"];
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException("App setting 'VideoOS.Reporting.Web.Plugin.DebugLogFileName' is missing.");
			}
			return text;
		}
	}

	public static string WinEventLogSourceName
	{
		get
		{
			string text = ConfigurationManager.AppSettings["VideoOS.Reporting.Web.Plugin.WinEventLogSourceName"];
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException("App setting 'VideoOS.Reporting.Web.Plugin.WinEventLogSourceName' is missing or empty.");
			}
			return text;
		}
	}
}
