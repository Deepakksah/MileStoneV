using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RemoveRoleClaimServerTask : ServerTask
{
	public string ClaimProvider
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClaimProvider");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClaimProvider", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ClaimName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClaimName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClaimName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ClaimValue
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClaimValue");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClaimValue", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public RemoveRoleClaimServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("RemoveRoleClaim"));
	}
}
