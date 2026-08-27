using System;

namespace VideoOS.Utilities.Telemetry;

public interface ILocalizedLoggerFactory : IDisposable
{
	ILocalizedLogger CreateLocalizedLogger(string name);
}
