using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChangeOverallSecurityPermissionsServerTask : ServerTask
{
	public Dictionary<string, string> SecurityNamespaceValues => base.Wrapper.GetEnumValues("SecurityNamespace");

	public string SecurityNamespace
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SecurityNamespace");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SecurityNamespace", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ChangeOverallSecurityPermissionsServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ChangeOverallSecurityPermissions"));
	}
}
