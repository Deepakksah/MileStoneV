using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Platform.Common.Telemetry;

public class PrivacySettingWrapper : IPrivacySettingWrapper
{
	public bool GetCollectTelemetryEnabled()
	{
		return new ToolOptionPrivacySetting(EnvironmentManager.Instance.MasterSite.ServerId, $"ToolOption[{ToolOptionFolder.PrivacySettingsToolOptionId}]").CollectTelemetryEnabled;
	}
}
