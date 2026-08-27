using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionPrivacySetting : IConfigurationItem
{
	public bool CollectUsageDataEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CollectUsageDataEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CollectUsageDataEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool CollectTelemetryEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CollectTelemetryEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CollectTelemetryEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionPrivacySetting(ServerId serverId, ConfigurationItem toolOptionPrivacySetting)
		: base(serverId, toolOptionPrivacySetting)
	{
	}

	public ToolOptionPrivacySetting(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionPrivacySetting(FQID fqid)
		: base(fqid)
	{
	}
}
