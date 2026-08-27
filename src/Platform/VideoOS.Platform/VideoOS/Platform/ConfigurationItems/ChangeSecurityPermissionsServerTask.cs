using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChangeSecurityPermissionsServerTask : ServerTask
{
	public string UserPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("UserPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("UserPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ChangeSecurityPermissionsServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ChangeSecurityPermissions"));
	}
}
