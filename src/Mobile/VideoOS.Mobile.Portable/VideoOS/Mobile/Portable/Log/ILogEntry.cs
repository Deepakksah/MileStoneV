namespace VideoOS.Mobile.Portable.Log;

public interface ILogEntry
{
	LogContent LogLevel { get; }

	string GetLogEntry();
}
