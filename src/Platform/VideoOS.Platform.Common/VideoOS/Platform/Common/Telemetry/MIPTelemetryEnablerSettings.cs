using System;

namespace VideoOS.Platform.Common.Telemetry;

public class MIPTelemetryEnablerSettings
{
	public TimeSpan RetryDelay = TimeSpan.FromHours(1.0);

	public int MaxConsecutiveErrors = 1;

	public TimeSpan RetryOnErrorDelay = TimeSpan.FromMinutes(1.0);
}
