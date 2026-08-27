using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChangePasswordStorageEncryptionServerTask : ServerTask
{
	public string OldPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("oldPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("oldPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string NewPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("newPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("newPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ChangePasswordStorageEncryptionServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ChangePasswordStorageEncryption"));
	}
}
