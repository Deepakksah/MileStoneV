using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareScanServerTask : ClassCollectionServerTask
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

	public Collection<string> HardwareDriverIds
	{
		get
		{
			return ConfigurationApiUtil.SeparatorStringToCollection(base.Wrapper.GetPropertyValue("HardwareDriverIds"));
		}
		set
		{
			base.Wrapper.SetPropertyValue("HardwareDriverIds", ConfigurationApiUtil.ToSeparatorString(value));
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

	public bool UseDefaultCredentials
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UseDefaultCredentials");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("UseDefaultCredentials", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public HardwareScanServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("HardwareScan"));
	}
}
