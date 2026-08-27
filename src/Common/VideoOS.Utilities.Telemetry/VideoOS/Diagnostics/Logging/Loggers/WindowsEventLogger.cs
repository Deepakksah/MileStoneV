using NSpring.Logging.Loggers;

namespace VideoOS.Diagnostics.Logging.Loggers;

public class WindowsEventLogger : LoggerBase
{
	public WindowsEventLogger(NSpring.Logging.Loggers.WindowsEventLogger loggerInstance)
		: base(loggerInstance)
	{
	}
}
