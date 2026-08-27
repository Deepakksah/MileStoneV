using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddRoleServerTask : ServerTask
{
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

	public bool DualAuthorizationRequired
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DualAuthorizationRequired");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("DualAuthorizationRequired", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool MakeUsersAnonymousDuringPTZSession
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MakeUsersAnonymousDuringPTZSession");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MakeUsersAnonymousDuringPTZSession", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AllowMobileClientLogOn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AllowMobileClientLogOn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AllowMobileClientLogOn", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AllowSmartClientLogOn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AllowSmartClientLogOn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AllowSmartClientLogOn", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool AllowWebClientLogOn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("AllowWebClientLogOn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("AllowWebClientLogOn", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string RoleDefaultTimeProfile
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RoleDefaultTimeProfile");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RoleDefaultTimeProfile", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string RoleClientLogOnTimeProfile
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RoleClientLogOnTimeProfile");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RoleClientLogOnTimeProfile", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string EvidenceLockProfile
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EvidenceLockProfile");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EvidenceLockProfile", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddRoleServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddRole"));
	}
}
