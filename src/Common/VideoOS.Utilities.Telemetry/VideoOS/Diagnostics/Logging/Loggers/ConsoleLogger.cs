using NSpring.Logging.Loggers;

namespace VideoOS.Diagnostics.Logging.Loggers;

public sealed class ConsoleLogger : LoggerBase
{
	public ConsoleLogger(NSpring.Logging.Loggers.ConsoleLogger loggerInstance)
		: base(loggerInstance)
	{
	}
}
