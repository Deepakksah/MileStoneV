using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Telemetry;

public interface ILocalizedLogger
{
	ILogger BaseLogger { get; }

	CultureInfo Culture { get; }

	void Log(LogLevel logLevel, Func<CultureInfo, LogItem[]> parameterFunc);
}
