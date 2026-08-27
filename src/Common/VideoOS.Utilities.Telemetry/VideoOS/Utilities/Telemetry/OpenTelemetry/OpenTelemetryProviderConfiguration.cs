using System.Collections.Generic;

namespace VideoOS.Utilities.Telemetry.OpenTelemetry;

public class OpenTelemetryProviderConfiguration
{
	public Dictionary<string, Exporter> Exporters { get; set; }
}
