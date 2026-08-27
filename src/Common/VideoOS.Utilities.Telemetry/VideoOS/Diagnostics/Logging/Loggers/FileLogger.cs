using NSpring.Logging.Loggers;

namespace VideoOS.Diagnostics.Logging.Loggers;

public sealed class FileLogger : LoggerBase
{
	public FileLogger(NSpring.Logging.Loggers.FileLogger loggerInstance)
		: base(loggerInstance)
	{
	}
}
