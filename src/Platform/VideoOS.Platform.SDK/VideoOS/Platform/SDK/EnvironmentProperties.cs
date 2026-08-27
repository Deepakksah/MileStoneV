using System;
using System.IO;
using System.Reflection;
using System.Xml;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK;

public class EnvironmentProperties : IEnvironmentProperties
{
	private IQueryConfigurationChanges _queryConfigChanges;

	IQueryConfigurationChanges IEnvironmentProperties.QueryConfigurationChanges => _queryConfigChanges;

	public bool TraceLogin { get; set; }

	public bool EnableConfigurationRefresh
	{
		get
		{
			return _queryConfigChanges.Enabled;
		}
		set
		{
			if (_queryConfigChanges.Enabled != value)
			{
				_queryConfigChanges.Enabled = value;
				if (value)
				{
					_queryConfigChanges.StartQuery(updateLoginState: false);
				}
				else
				{
					_queryConfigChanges.StopQuery(updateLoginState: false);
				}
			}
		}
	}

	[Obsolete("Deprecated. Use masterOnly property passed in SDK calls to control if sub-sites should be loaded after the initial logon.")]
	public bool LoadSubSitesAfterwards { get; set; }

	public int ConfigurationRefreshIntervalInMs
	{
		get
		{
			return _queryConfigChanges.ConfigurationRefreshIntervalInMs;
		}
		set
		{
			_queryConfigChanges.ConfigurationRefreshIntervalInMs = value;
		}
	}

	public bool KeepLoginServerUriScheme { get; set; }

	public int LoginServerSvcTimeoutInSeconds { get; set; }

	public EnvironmentProperties(IQueryConfigurationChanges queryConfigChanges)
	{
		_queryConfigChanges = queryConfigChanges;
		ConfigurationRefreshIntervalInMs = 5000;
		try
		{
			string text = Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().Location).Path)), "MIPSettings.xml");
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				ServerTrace.TraceServerCommunication = GetBoolSetting(xmlDocument, "Settings/LoginTrace", defaultResult: false);
				KeepLoginServerUriScheme = GetBoolSetting(xmlDocument, "Settings/KeepLoginServerUriScheme", defaultResult: false);
				LoginServerSvcTimeoutInSeconds = GetIntSetting(xmlDocument, "Settings/LoginServerSvcTimeoutInSeconds", 0);
			}
		}
		catch (Exception)
		{
			ServerTrace.TraceServerCommunication = false;
		}
	}

	private static bool GetBoolSetting(XmlDocument doc, string nodePath, bool defaultResult)
	{
		bool result = defaultResult;
		XmlNode xmlNode = doc.SelectSingleNode(nodePath);
		if (xmlNode != null)
		{
			bool.TryParse(xmlNode.InnerText, out result);
		}
		return result;
	}

	private static int GetIntSetting(XmlDocument doc, string nodePath, int defaultResult)
	{
		int result = defaultResult;
		XmlNode xmlNode = doc.SelectSingleNode(nodePath);
		if (xmlNode != null)
		{
			int.TryParse(xmlNode.InnerText, out result);
		}
		return result;
	}
}
