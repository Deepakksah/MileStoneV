using VideoOS.Telemetry;

namespace VideoOS.Management.Server.Telemetry;

public class TelemetryKeyProvider : ITelemetryKeyProvider
{
	private readonly bool _isProductionEnv;

	protected static string ProductionTelemetryKey { get; private set; } = "InstrumentationKey=ad3a3fc5-79ae-4db9-bc55-3489103041be;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/";

	protected static string DevelopmentTelemetryKey { get; private set; } = "InstrumentationKey=9e8d6ce0-c936-4ded-b0f5-e97f7740ed83;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/";

	public TelemetryKeyProvider(bool isProductionEnv)
	{
		_isProductionEnv = isProductionEnv;
	}

	public string GetKey()
	{
		if (!_isProductionEnv)
		{
			return DevelopmentTelemetryKey;
		}
		return ProductionTelemetryKey;
	}
}
