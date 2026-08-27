using System;

namespace VideoOS.Management.Server;

[Flags]
public enum GeminiDatabaseLoggingOptions
{
	None = 0,
	CommandText = 1,
	Statistics = 2,
	All = 3
}
