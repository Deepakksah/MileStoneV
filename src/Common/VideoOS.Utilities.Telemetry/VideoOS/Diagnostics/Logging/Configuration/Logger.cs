using System;
using System.Configuration;
using System.Xml;

namespace VideoOS.Diagnostics.Logging.Configuration;

public class Logger
{
	private string _name;

	private AppenderCollection _appenders;

	public string Name => _name;

	public AppenderCollection Appenders => _appenders;

	public Logger(XmlNode loggerNode)
	{
		if (loggerNode == null)
		{
			throw new NullReferenceException("Xml node passed to Logger constructor cannot be null");
		}
		XmlNode namedItem = loggerNode.Attributes.GetNamedItem("name");
		if (namedItem == null)
		{
			throw new ConfigurationErrorsException("A 'Logger' was defined without a 'name' attribute in the configuration file. A 'Logger' must have a name defined.");
		}
		if (namedItem.Value.Length == 0)
		{
			throw new ConfigurationErrorsException("A 'Logger' was defined without an empty 'name' attribute in the configuration file. A 'Logger' must have a name defined.");
		}
		_name = namedItem.Value;
		_appenders = new AppenderCollection();
		foreach (XmlNode item in loggerNode.SelectNodes("Appender"))
		{
			Appender value = new Appender(item, this);
			_appenders.Add(value);
		}
	}
}
