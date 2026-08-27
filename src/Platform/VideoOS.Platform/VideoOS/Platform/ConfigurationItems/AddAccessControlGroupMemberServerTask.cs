using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddAccessControlGroupMemberServerTask : ServerTask
{
	public string ItemSelection
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ItemSelection");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ItemSelection", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddAccessControlGroupMemberServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddAccessControlGroupMember"));
	}
}
