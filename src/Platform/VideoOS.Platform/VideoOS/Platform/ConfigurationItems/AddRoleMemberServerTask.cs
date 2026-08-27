using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddRoleMemberServerTask : ServerTask
{
	public string Sid
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Sid");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Sid", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddRoleMemberServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddRoleMember"));
	}
}
