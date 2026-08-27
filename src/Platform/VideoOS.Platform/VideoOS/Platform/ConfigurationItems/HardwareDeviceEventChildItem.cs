using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDeviceEventChildItem : IConfigurationChildItem
{
	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public bool EventUsed
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EventUsed");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EventUsed", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string EventIndex
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EventIndex");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EventIndex", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public HardwareDeviceEventChildItem(ServerId serverId, ConfigurationItem hardwareDeviceEventChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, hardwareDeviceEventChildItem, parentWrapper)
	{
	}

	public HardwareDeviceEventChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDeviceEventChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
