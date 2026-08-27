using VideoOS.Platform.License;

namespace VideoOS.Platform.Common.Telemetry;

public class TelemetryKeyProvider : TelemetryKeyProviderBase
{
	private LicenseManager licenseManager = EnvironmentManager.Instance.LicenseManager;

	public override string GetKey()
	{
		if (string.IsNullOrEmpty(licenseManager.ProductionEnvironment))
		{
			return TelemetryKeyProviderBase.ProductionTelemetryKey;
		}
		return TelemetryKeyProviderBase.DevelopmentTelemetryKey;
	}
}
