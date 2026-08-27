using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientLogOnSupportedServerTask : ServerTask
{
	public Dictionary<string, string> ClientApplicationTypeValues => base.Wrapper.GetEnumValues("ClientApplicationType");

	public string ClientApplicationType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClientApplicationType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClientApplicationType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string UserName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("UserName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("UserName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ClientLogOnSupportedServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ClientLogOnSupported"));
	}
}
