using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChangeLicenseServerTask : ServerTask
{
	public string License
	{
		get
		{
			return base.Wrapper.GetPropertyValue("License");
		}
		set
		{
			base.Wrapper.SetPropertyValue("License", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ChangeLicenseServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ChangeLicense"));
	}
}
