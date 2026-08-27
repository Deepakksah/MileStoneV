using VideoOS.Telemetry;

namespace VideoOS.Management.Server.Telemetry;

public class TelemetryEnabler : ITelemetryEnabler
{
	private readonly IGeminiRequestContext requestContext;

	public bool IsTelemetryEnabled
	{
		get
		{
			if (bool.TryParse(SystemSetting.GetSystemSetting(requestContext, "CollectTelemetryEnabled"), out var result))
			{
				return result;
			}
			return false;
		}
	}

	public TelemetryEnabler(IGeminiRequestContext requestContext)
	{
		this.requestContext = requestContext;
	}
}
