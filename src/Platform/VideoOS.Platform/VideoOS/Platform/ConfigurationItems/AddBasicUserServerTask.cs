using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddBasicUserServerTask : ServerTask
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

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string Name
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Name");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Name", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Description
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Description");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Description", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool CanChangePassword
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CanChangePassword");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CanChangePassword", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ForcePasswordChange
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ForcePasswordChange");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ForcePasswordChange", value.ToString());
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

	public Dictionary<string, string> StatusValues => base.Wrapper.GetEnumValues("Status");

	public string Status
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Status");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Status", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime LockoutEnd
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LockoutEnd"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public AddBasicUserServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddBasicUser"));
	}
}
