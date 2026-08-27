using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK;

public interface IEnvironmentProperties
{
	int ConfigurationRefreshIntervalInMs { get; set; }

	bool EnableConfigurationRefresh { get; set; }

	bool TraceLogin { get; set; }

	IQueryConfigurationChanges QueryConfigurationChanges { get; }

	bool LoadSubSitesAfterwards { get; set; }

	bool KeepLoginServerUriScheme { get; set; }
}
