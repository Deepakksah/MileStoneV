namespace VideoOS.Management.Server;

public static class GeminiEventId
{
	public static readonly int DefaultEventId = 0;

	public static readonly int ExceptionBaseEventId = 1000;

	public static readonly int DefaultExceptionEventId = ExceptionBaseEventId;

	public static readonly int ThreadAbortException = ExceptionBaseEventId + 1;

	public static readonly int UnexpectedDatabaseResultException = ExceptionBaseEventId + 2;

	public static readonly int UnhandledExceptionOccurred = ExceptionBaseEventId + 3;

	public static readonly int TooManyItemsException = ExceptionBaseEventId + 4;

	public static readonly int UnexpectedDatabaseResult = ExceptionBaseEventId + 5;

	public static readonly int DatabaseConnectionException = ExceptionBaseEventId + 6;

	public static readonly int WarningBaseEventId = 2000;

	public static readonly int InvalidTracingConfiguration = WarningBaseEventId + 1;

	public static readonly int InformationalBaseEventId = 3000;

	public static readonly int DefaultInformationalEventId = InformationalBaseEventId;

	public static readonly int ApplicationStarted = InformationalBaseEventId + 1;

	public static readonly int ApplicationStopped = InformationalBaseEventId + 2;

	public static readonly int DataChangedEventException = InformationalBaseEventId + 200;
}
