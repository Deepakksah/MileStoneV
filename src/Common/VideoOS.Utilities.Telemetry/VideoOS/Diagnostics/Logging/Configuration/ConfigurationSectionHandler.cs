using System.Configuration;
using System.Reflection;
using System.Xml;

namespace VideoOS.Diagnostics.Logging.Configuration;

[Obfuscation(Exclude = true)]
public sealed class ConfigurationSectionHandler : IConfigurationSectionHandler
{
	[Obfuscation(Exclude = true)]
	public object Create(object parent, object configContext, XmlNode section)
	{
		LogSettings logSettings = new LogSettings();
		foreach (XmlNode item in section.SelectNodes("Logger"))
		{
			Logger logger = new Logger(item);
			logSettings.Loggers[logger.Name] = logger;
		}
		return logSettings;
	}
}
