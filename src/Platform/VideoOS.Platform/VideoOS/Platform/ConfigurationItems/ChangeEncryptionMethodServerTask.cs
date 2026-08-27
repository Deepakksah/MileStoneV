using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ChangeEncryptionMethodServerTask : ServerTask
{
	public Dictionary<string, string> EncryptionMethodValues => base.Wrapper.GetEnumValues("EncryptionMethod");

	public string EncryptionMethod
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EncryptionMethod");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EncryptionMethod", value);
			base.Wrapper.IsDirty = true;
		}
	}

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

	public ChangeEncryptionMethodServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ChangeEncryptionMethod"));
	}
}
