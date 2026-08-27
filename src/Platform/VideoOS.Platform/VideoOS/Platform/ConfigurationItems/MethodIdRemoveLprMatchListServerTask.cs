using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MethodIdRemoveLprMatchListServerTask : ServerTask
{
	public Dictionary<string, string> IdValues => base.Wrapper.GetEnumValues("Id");

	public string Id
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Id");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Id", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public MethodIdRemoveLprMatchListServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MethodIdRemoveLprMatchList"));
	}
}
