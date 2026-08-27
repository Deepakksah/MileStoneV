using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Role : IConfigurationItem
{
	public ClaimFolder ClaimFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ClaimFolder")
				{
					return new ClaimFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public UserFolder UserFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "UserFolder")
				{
					return new UserFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public Dictionary<string, string> RoleTypeValues => base.Wrapper.GetEnumValues("RoleType");

	public string RoleType => base.Wrapper.GetPropertyValue("RoleType");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
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

	public string ClientProfile => base.Wrapper.GetPropertyValue("ClientProfile");

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

	public Role(ServerId serverId, ConfigurationItem role)
		: base(serverId, role)
	{
	}

	public Role(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Role(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask SetClientTimeProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SetClientTimeProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("SetClientTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'SetClientTimeProfile':" + ex.Message);
		}
		return null;
	}

	public SetClientTimeProfileServerTask SetClientTimeProfile()
	{
		try
		{
			return new SetClientTimeProfileServerTask(base.Wrapper.ExecuteMethod("SetClientTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'SetClientTimeProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask GetClientTimeProfile()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("GetClientTimeProfile").ExecuteMethod("GetClientTimeProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'GetClientTimeProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask SetClientProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SetClientProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("SetClientProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'SetClientProfile':" + ex.Message);
		}
		return null;
	}

	public SetClientProfileServerTask SetClientProfile()
	{
		try
		{
			return new SetClientProfileServerTask(base.Wrapper.ExecuteMethod("SetClientProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'SetClientProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangeOverallSecurityPermissions(string securityNamespace)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeOverallSecurityPermissions");
			configurationApiItemWrapper.SetProperty("SecurityNamespace", securityNamespace);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeOverallSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'ChangeOverallSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ChangeOverallSecurityPermissionsServerTask ChangeOverallSecurityPermissions()
	{
		try
		{
			return new ChangeOverallSecurityPermissionsServerTask(base.Wrapper.ExecuteMethod("ChangeOverallSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Role", "Unable to call method 'ChangeOverallSecurityPermissions':" + ex.Message);
		}
		return null;
	}
}
