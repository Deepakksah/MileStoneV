namespace VideoOS.Common.Integration;

public class LogEntryDefinitionState : ILogEntryDefinition
{
	private LogEntryDefinition _logDefinitionSetState;

	private LogEntryDefinition[] _logDefinitionsClearStates;

	private ILogRecurrence _recurrenceHandler;

	public LogEntryDefinition LogDefinitionSetState => _logDefinitionSetState;

	public LogEntryDefinition[] LogDefinitionsClearStates => _logDefinitionsClearStates;

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

	public LogEntryDefinitionState(LogEntryDefinition logDefinitionSetState, ILogRecurrence recurrenceHandler)
	{
		_logDefinitionSetState = logDefinitionSetState;
		_logDefinitionsClearStates = new LogEntryDefinition[0];
		_recurrenceHandler = recurrenceHandler;
	}

	public LogEntryDefinitionState(LogEntryDefinition logDefinitionSetState, LogEntryDefinition logDefinitionClearState, ILogRecurrence recurrenceHandler)
	{
		_logDefinitionSetState = logDefinitionSetState;
		_logDefinitionsClearStates = new LogEntryDefinition[1] { logDefinitionClearState };
		_recurrenceHandler = recurrenceHandler;
	}

	public LogEntryDefinitionState(LogEntryDefinition logDefinitionSetState, LogEntryDefinition[] logDefinitionsClearState, ILogRecurrence recurrenceHandler)
	{
		_logDefinitionSetState = logDefinitionSetState;
		_logDefinitionsClearStates = logDefinitionsClearState;
		_recurrenceHandler = recurrenceHandler;
	}

	public bool IsClearState(int logId)
	{
		return _logDefinitionSetState.LogId != logId;
	}

	public LogEntryDefinition GetLogEntryDefinition(int logId)
	{
		if (logId == _logDefinitionSetState.LogId)
		{
			return _logDefinitionSetState;
		}
		for (int i = 0; i < _logDefinitionsClearStates.Length; i++)
		{
			if (logId == _logDefinitionsClearStates[i].LogId)
			{
				return _logDefinitionsClearStates[i];
			}
		}
		return null;
	}
}
