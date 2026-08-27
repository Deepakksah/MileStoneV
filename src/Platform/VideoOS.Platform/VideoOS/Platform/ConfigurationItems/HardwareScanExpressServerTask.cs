using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareScanExpressServerTask : ClassCollectionServerTask
{
	public string PrimaryUserName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PrimaryUserName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrimaryUserName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string PrimaryPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PrimaryPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrimaryPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string SecondaryUserName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SecondaryUserName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SecondaryUserName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string SecondaryPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SecondaryPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SecondaryPassword", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string TertiaryUserName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TertiaryUserName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TertiaryUserName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string TertiaryPassword
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TertiaryPassword");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TertiaryPassword", value);
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

	public bool UseSecurePort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UseSecurePort");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("UseSecurePort", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public HardwareScanExpressServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("HardwareScanExpress"));
	}
}
