using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ReplaceHardwareServerTask : ServerTask
{
	public string Address
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Address");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Address", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Port
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Port");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Port", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool UseHttps
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UseHttps");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("UseHttps", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int HttpsPort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("HttpsPort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("HttpsPort", value.ToString(CultureInfo.InvariantCulture));
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

	public Dictionary<string, string> DriverValues => base.Wrapper.GetEnumValues("Driver");

	public string Driver
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Driver");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Driver", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string CustomDriverData
	{
		get
		{
			return base.Wrapper.GetPropertyValue("CustomDriverData");
		}
		set
		{
			base.Wrapper.SetPropertyValue("CustomDriverData", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AllowDeletingDisabledDevices
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AllowDeletingDisabledDevices");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AllowDeletingDisabledDevices", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ReplaceHardwareServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ReplaceHardware"));
	}
}
