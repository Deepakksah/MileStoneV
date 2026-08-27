using VideoOS.Platform.Telemetry;
using VideoOS.Telemetry;

namespace VideoOS.Platform.Common.Telemetry;

public class TelemetryClientControl : ClientControl
{
	private readonly TelemetryClient _telemetryClient;

	public const int FlushMaximumWaitMilliseconds = 600;

	public override TelemetryClient TelemetryClient => _telemetryClient;

	public override bool Initialized => false;

	public TelemetryClientControl(string environmentId, ITelemetryManager telemetryManager)
	{
		_telemetryClient = new VideoOSTelemetryClient(telemetryManager, environmentId);
	}
}
