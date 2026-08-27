using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class GetInternalUnitPathServerTask : ServerTask
{
	public string ExternalId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExternalId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExternalId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public GetInternalUnitPathServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("GetInternalUnitPath"));
	}
}
