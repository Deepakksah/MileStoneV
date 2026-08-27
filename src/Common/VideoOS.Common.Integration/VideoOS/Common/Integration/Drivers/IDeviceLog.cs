using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDeviceLog
{
	bool IsLogDebugEnabled { get; }

	void LogDebug(string entry);

	void LogError(string entry);

	void LogWarning(string entry);

	void LogInfo(string entry);

	void LogMethod(bool begin);

	void LogException(Exception exception);

	void LogException(Exception exception, string comment);
}
