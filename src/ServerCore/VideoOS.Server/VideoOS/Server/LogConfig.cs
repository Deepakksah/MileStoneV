using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public sealed class LogConfig : ILogConfig
{
	public string CacheFile { get; set; }

	public int BundleMaxCount { get; private set; }

	public TimeSpan BundleInterval { get; private set; }

	public int IdpBundleMaxCount { get; private set; }

	public TimeSpan IdpBundleInterval { get; private set; }

	public Dictionary<LogType, LogDefinition> Logs { get; private set; }

	public LogConfig()
	{
		Logs = new Dictionary<LogType, LogDefinition>();
		CacheFile = ".\\Logs.dat";
		BundleMaxCount = 500;
		BundleInterval = TimeSpan.FromMilliseconds(250.0);
		IdpBundleMaxCount = 500;
		IdpBundleInterval = TimeSpan.FromSeconds(10.0);
	}

	public void Initialize(XmlElement logElement)
	{
		if (logElement != null)
		{
			XmlElement xmlElement = logElement["IdpBundleMaxCount"];
			if (xmlElement != null && !string.IsNullOrEmpty(xmlElement.InnerText) && int.TryParse(xmlElement.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				IdpBundleMaxCount = result;
			}
			XmlElement xmlElement2 = logElement["IdpBundleIntervalSeconds"];
			if (xmlElement2 != null && !string.IsNullOrEmpty(xmlElement2.InnerText) && int.TryParse(xmlElement2.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
			{
				IdpBundleInterval = TimeSpan.FromSeconds(result2);
			}
		}
	}
}
