using NSpring.Logging;
using NSpring.Logging.Loggers;

namespace VideoOS.Diagnostics.Logging.Loggers;

public sealed class CompositeLogger : LoggerBase
{
	public CompositeLogger(NSpring.Logging.Loggers.CompositeLogger loggerInstance)
		: base(loggerInstance)
	{
	}

	public void AddChild(Logger childLogger)
	{
		_logger.AddChild(childLogger);
	}
}
