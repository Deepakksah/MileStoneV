namespace VideoOS.Common.Integration;

public interface ILogReceiver
{
	void Initialize();

	void Close();

	void AddLogEntry(LogEntry logEntry);

	void AddLogEntry(LogEntry logEntry, bool skipQueue);

	void AddLogEntries(LogEntry[] logEntry);

	void AddLogEntries(LogEntry[] logEntry, bool skipQueue);
}
