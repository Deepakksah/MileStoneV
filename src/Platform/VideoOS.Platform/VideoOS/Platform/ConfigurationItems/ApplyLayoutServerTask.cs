using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ApplyLayoutServerTask : ServerTask
{
	public string LayoutPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ApplyLayoutServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ApplyLayout"));
	}
}
