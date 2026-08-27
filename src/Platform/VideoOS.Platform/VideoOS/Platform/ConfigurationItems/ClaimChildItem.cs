using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClaimChildItem : IConfigurationChildItem
{
	public string ClaimProvider => base.Wrapper.GetPropertyValue("ClaimProvider");

	public string ClaimName => base.Wrapper.GetPropertyValue("ClaimName");

	public string ClaimValue => base.Wrapper.GetPropertyValue("ClaimValue");

	public ClaimChildItem(ServerId serverId, ConfigurationItem claimChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, claimChildItem, parentWrapper)
	{
	}

	public ClaimChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClaimChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
