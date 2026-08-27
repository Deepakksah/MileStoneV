using System.Configuration;

namespace VideoOS.Diagnostics.Logging.Configuration;

public class LogSettings : ConfigurationSection
{
	private LoggerCollection _loggerCollection;

	public LoggerCollection Loggers => _loggerCollection;

	public LogSettings()
	{
		_loggerCollection = new LoggerCollection();
	}
}
