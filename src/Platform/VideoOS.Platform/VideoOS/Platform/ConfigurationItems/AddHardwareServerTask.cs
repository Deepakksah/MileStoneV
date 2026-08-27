using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddHardwareServerTask : ServerTask
{
	public string Password
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Password");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Password", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string HardwareAddress
	{
		get
		{
			return base.Wrapper.GetPropertyValue("HardwareAddress");
		}
		set
		{
			base.Wrapper.SetPropertyValue("HardwareAddress", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string HardwareDriverPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("HardwareDriverPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("HardwareDriverPath", value);
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

	public string CustomData
	{
		get
		{
			return base.Wrapper.GetPropertyValue("CustomData");
		}
		set
		{
			base.Wrapper.SetPropertyValue("CustomData", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddHardwareServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddHardware"));
	}
}
