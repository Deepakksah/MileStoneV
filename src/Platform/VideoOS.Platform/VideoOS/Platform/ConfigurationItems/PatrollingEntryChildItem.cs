using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PatrollingEntryChildItem : IConfigurationChildItem
{
	public int Order
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Order");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Order", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> PresetIdValues => base.Wrapper.GetEnumValues("PresetId");

	public string PresetId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PresetId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PresetId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double WaitTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("WaitTime");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("WaitTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public PatrollingEntryChildItem(ServerId serverId, ConfigurationItem patrollingEntryChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, patrollingEntryChildItem, parentWrapper)
	{
	}

	public PatrollingEntryChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PatrollingEntryChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
