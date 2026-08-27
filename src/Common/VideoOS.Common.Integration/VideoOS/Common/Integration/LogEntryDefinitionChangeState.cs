namespace VideoOS.Common.Integration;

public class LogEntryDefinitionChangeState : ILogEntryDefinition
{
	private LogEntryDefinition[] _logEntryDefinitions;

	private ILogRecurrence _recurrenceHandler;

	public LogEntryDefinition[] LogEntryDefinitions => _logEntryDefinitions;

	public ILogRecurrence RecurrenceHandler
	{
		get
		{
			return _recurrenceHandler;
		}
		set
		{
			_recurrenceHandler = value;
		}
	}

	public LogEntryDefinitionChangeState(LogEntryDefinition[] logDefinitions, ILogRecurrence recurrenceHandler)
	{
		_logEntryDefinitions = logDefinitions;
		_recurrenceHandler = recurrenceHandler;
	}

	public LogEntryDefinition GetLogEntryDefinition(int logId)
	{
		LogEntryDefinition[] logEntryDefinitions = _logEntryDefinitions;
		foreach (LogEntryDefinition logEntryDefinition in logEntryDefinitions)
		{
			if (logEntryDefinition.LogId == logId)
			{
				return logEntryDefinition;
			}
		}
		return null;
	}
}
