using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClearViewItemServerTask : ServerTask
{
	public string ViewItemId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewItemId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewItemId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ClientId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClientId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClientId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ClearViewItemServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ClearViewItem"));
	}
}
