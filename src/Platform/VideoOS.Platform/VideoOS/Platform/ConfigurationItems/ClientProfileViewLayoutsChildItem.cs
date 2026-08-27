using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileViewLayoutsChildItem : IConfigurationChildItem
{
	public string DisabledViewLayouts
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DisabledViewLayouts");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DisabledViewLayouts", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientProfileViewLayoutsChildItem(ServerId serverId, ConfigurationItem clientProfileViewLayoutsChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, clientProfileViewLayoutsChildItem, parentWrapper)
	{
	}

	public ClientProfileViewLayoutsChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileViewLayoutsChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
