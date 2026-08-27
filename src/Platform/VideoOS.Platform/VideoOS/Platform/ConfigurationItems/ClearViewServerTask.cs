using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClearViewServerTask : ServerTask
{
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

	public ClearViewServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ClearView"));
	}
}
