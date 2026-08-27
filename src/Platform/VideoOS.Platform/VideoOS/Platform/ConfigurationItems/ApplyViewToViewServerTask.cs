using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ApplyViewToViewServerTask : ServerTask
{
	public string ViewToApply
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewToApply");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewToApply", value);
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

	public ApplyViewToViewServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ApplyViewToView"));
	}
}
