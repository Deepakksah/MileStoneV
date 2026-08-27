using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PTZSessionTimeoutChildItem : IConfigurationChildItem
{
	public int ManualPTZTimeout
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ManualPTZTimeout");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManualPTZTimeout", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int PausePatrollingTimeout
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PausePatrollingTimeout");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PausePatrollingTimeout", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int ReservedPTZTimeout
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ReservedPTZTimeout");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ReservedPTZTimeout", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public PTZSessionTimeoutChildItem(ServerId serverId, ConfigurationItem ptzSessionTimeoutChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, ptzSessionTimeoutChildItem, parentWrapper)
	{
	}

	public PTZSessionTimeoutChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PTZSessionTimeoutChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
