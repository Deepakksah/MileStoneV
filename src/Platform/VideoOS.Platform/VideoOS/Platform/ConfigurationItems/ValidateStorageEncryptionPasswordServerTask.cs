using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ValidateStorageEncryptionPasswordServerTask : ServerTask
{
	public SecureString SecurePassword
	{
		get
		{
			return base.Wrapper.GetPropertySecureValue("Password");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Password", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ValidateStorageEncryptionPasswordServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ValidateStorageEncryptionPassword"));
	}
}
