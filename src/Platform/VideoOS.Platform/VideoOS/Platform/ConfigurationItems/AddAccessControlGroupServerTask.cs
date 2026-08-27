using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddAccessControlGroupServerTask : ServerTask
{
	public string GroupName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GroupName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GroupName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string GroupDescription
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GroupDescription");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GroupDescription", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddAccessControlGroupServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddAccessControlGroup"));
	}
}
