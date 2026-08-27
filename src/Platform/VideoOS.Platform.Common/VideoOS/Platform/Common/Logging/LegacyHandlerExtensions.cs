using VideoOS.Log;
using VideoOS.LogServer.Communication.LogReaderServiceReference;

namespace VideoOS.Platform.Common.Logging;

public static class LegacyHandlerExtensions
{
	public static Severity ToClientSeverity(this string severity)
	{
		return severity switch
		{
			"Error" => Severity.Error, 
			"Warning" => Severity.Warning, 
			"Info" => Severity.Info, 
			"Debug" => Severity.Debug, 
			_ => Severity.Debug, 
		};
	}

	public static Status ToClientStatus(this string status)
	{
		return status switch
		{
			"Success" => Status.Success, 
			"Failure" => Status.Failure, 
			"StatusQuo" => Status.StatusQuo, 
			_ => Status.StatusQuo, 
		};
	}

	public static LogType ToLogReaderLogType(this string logType)
	{
		return logType switch
		{
			"Audit" => LogType.Audit, 
			"Rules" => LogType.Rule, 
			"System" => LogType.System, 
			_ => LogType.System, 
		};
	}
}
