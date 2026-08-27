using System.Collections.Generic;

namespace VideoOS.Utilities.Telemetry;

public class LoggerConfiguration
{
	public string LogLevel { get; set; }

	public List<string> Providers { get; set; }
}
