using System;
using System.Configuration;

namespace VideoOS.Management.Server;

public class Config : IConfig
{
	private static Config _instance;

	public static Config Instance => _instance ?? (_instance = new Config());

	public string GetAppSetting(string name, string defaultValue)
	{
		if (name == null)
		{
			throw new ArgumentNullException(name);
		}
		string text = ConfigurationManager.AppSettings.Get(name);
		if (string.IsNullOrEmpty(text))
		{
			return defaultValue;
		}
		return text;
	}
}
