using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RoleFolder : IConfigurationItem
{
	public ICollection<Role> Roles
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Role> list = new List<Role>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Role")
				{
					list.Add(new Role(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public RoleFolder(ServerId serverId, ConfigurationItem roleFolder)
		: base(serverId, roleFolder)
	{
	}

	public RoleFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RoleFolder(FQID fqid)
		: base(fqid)
	{
	}

	public RoleFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/RoleFolder")
	{
	}

	public ServerTask AddRole(string name, string description, bool dualAuthorizationRequired, bool makeUsersAnonymousDuringPTZSession, bool allowMobileClientLogOn, bool allowSmartClientLogOn, bool allowWebClientLogOn, string roleDefaultTimeProfile, string roleClientLogOnTimeProfile, string evidenceLockProfile)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRole");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DualAuthorizationRequired", dualAuthorizationRequired.ToString());
			configurationApiItemWrapper.SetProperty("MakeUsersAnonymousDuringPTZSession", makeUsersAnonymousDuringPTZSession.ToString());
			configurationApiItemWrapper.SetProperty("AllowMobileClientLogOn", allowMobileClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("AllowSmartClientLogOn", allowSmartClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("AllowWebClientLogOn", allowWebClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("RoleDefaultTimeProfile", roleDefaultTimeProfile);
			configurationApiItemWrapper.SetProperty("RoleClientLogOnTimeProfile", roleClientLogOnTimeProfile);
			configurationApiItemWrapper.SetProperty("EvidenceLockProfile", evidenceLockProfile);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'AddRole':" + ex.Message);
		}
		return null;
	}

	public AddRoleServerTask AddRole()
	{
		try
		{
			return new AddRoleServerTask(base.Wrapper.ExecuteMethod("AddRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'AddRole':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveRole(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveRole");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'RemoveRole':" + ex.Message);
		}
		return null;
	}

	public RemoveRoleServerTask RemoveRole()
	{
		try
		{
			return new RemoveRoleServerTask(base.Wrapper.ExecuteMethod("RemoveRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'RemoveRole':" + ex.Message);
		}
		return null;
	}

	public ServerTask CopyRole(string itemSelection, string name, string description)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("CopyRole");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("CopyRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'CopyRole':" + ex.Message);
		}
		return null;
	}

	public CopyRoleServerTask CopyRole()
	{
		try
		{
			return new CopyRoleServerTask(base.Wrapper.ExecuteMethod("CopyRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'CopyRole':" + ex.Message);
		}
		return null;
	}

	public ServerTask QueryUsersWithAccess(string queryUsersWithAccessPath, bool includeAdministrators, bool expandGroups, bool includeGroups)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("QueryUsersWithAccess");
			configurationApiItemWrapper.SetProperty("QueryUsersWithAccessPath", queryUsersWithAccessPath);
			configurationApiItemWrapper.SetProperty("IncludeAdministrators", includeAdministrators.ToString());
			configurationApiItemWrapper.SetProperty("ExpandGroups", expandGroups.ToString());
			configurationApiItemWrapper.SetProperty("IncludeGroups", includeGroups.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("QueryUsersWithAccess"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'QueryUsersWithAccess':" + ex.Message);
		}
		return null;
	}

	public QueryUsersWithAccessServerTask QueryUsersWithAccess()
	{
		try
		{
			return new QueryUsersWithAccessServerTask(base.Wrapper.ExecuteMethod("QueryUsersWithAccess"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'QueryUsersWithAccess':" + ex.Message);
		}
		return null;
	}

	public ServerTask QueryIsPermittedTimeBasedAction(string relatedObjectPath, string actionId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("QueryIsPermittedTimeBasedAction");
			configurationApiItemWrapper.SetProperty("RelatedObjectPath", relatedObjectPath);
			configurationApiItemWrapper.SetProperty("ActionId", actionId);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("QueryIsPermittedTimeBasedAction"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'QueryIsPermittedTimeBasedAction':" + ex.Message);
		}
		return null;
	}

	public QueryIsPermittedTimeBasedActionServerTask QueryIsPermittedTimeBasedAction()
	{
		try
		{
			return new QueryIsPermittedTimeBasedActionServerTask(base.Wrapper.ExecuteMethod("QueryIsPermittedTimeBasedAction"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'QueryIsPermittedTimeBasedAction':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddRole(string name, string description, bool dualAuthorizationRequired, bool makeUsersAnonymousDuringPTZSession, bool allowMobileClientLogOn, bool allowSmartClientLogOn, bool allowWebClientLogOn)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRole");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DualAuthorizationRequired", dualAuthorizationRequired.ToString());
			configurationApiItemWrapper.SetProperty("MakeUsersAnonymousDuringPTZSession", makeUsersAnonymousDuringPTZSession.ToString());
			configurationApiItemWrapper.SetProperty("AllowMobileClientLogOn", allowMobileClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("AllowSmartClientLogOn", allowSmartClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("AllowWebClientLogOn", allowWebClientLogOn.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'AddRole':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddRole(string name, string description, bool dualAuthorizationRequired, bool makeUsersAnonymousDuringPTZSession, bool allowMobileClientLogOn, bool allowSmartClientLogOn, bool allowWebClientLogOn, string roleDefaultTimeProfile, string roleClientLogOnTimeProfile)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRole");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DualAuthorizationRequired", dualAuthorizationRequired.ToString());
			configurationApiItemWrapper.SetProperty("MakeUsersAnonymousDuringPTZSession", makeUsersAnonymousDuringPTZSession.ToString());
			configurationApiItemWrapper.SetProperty("AllowMobileClientLogOn", allowMobileClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("AllowSmartClientLogOn", allowSmartClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("AllowWebClientLogOn", allowWebClientLogOn.ToString());
			configurationApiItemWrapper.SetProperty("RoleDefaultTimeProfile", roleDefaultTimeProfile);
			configurationApiItemWrapper.SetProperty("RoleClientLogOnTimeProfile", roleClientLogOnTimeProfile);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRole"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RoleFolder", "Unable to call method 'AddRole':" + ex.Message);
		}
		return null;
	}
}
