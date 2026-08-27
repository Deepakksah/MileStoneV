using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionCustomerDashbord : IConfigurationItem
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

	public ToolOptionCustomerDashbord(ServerId serverId, ConfigurationItem toolOptionCustomerDashbord)
		: base(serverId, toolOptionCustomerDashbord)
	{
	}

	public ToolOptionCustomerDashbord(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionCustomerDashbord(FQID fqid)
		: base(fqid)
	{
	}
}
