using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AccessControlGroupFolder : IConfigurationItem
{
	public ICollection<AccessControlGroup> AccessControlGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<AccessControlGroup> list = new List<AccessControlGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "AccessControlGroup")
				{
					list.Add(new AccessControlGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public AccessControlGroupFolder(ServerId serverId, ConfigurationItem accessControlGroupFolder)
		: base(serverId, accessControlGroupFolder)
	{
	}

	public AccessControlGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AccessControlGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public AccessControlGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/AccessControlGroupFolder")
	{
	}

	public ServerTask AddAccessControlGroup(string groupName, string groupDescription)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAccessControlGroup");
			configurationApiItemWrapper.SetProperty("GroupName", groupName);
			configurationApiItemWrapper.SetProperty("GroupDescription", groupDescription);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAccessControlGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlGroupFolder", "Unable to call method 'AddAccessControlGroup':" + ex.Message);
		}
		return null;
	}

	public AddAccessControlGroupServerTask AddAccessControlGroup()
	{
		try
		{
			return new AddAccessControlGroupServerTask(base.Wrapper.ExecuteMethod("AddAccessControlGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlGroupFolder", "Unable to call method 'AddAccessControlGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveAccessControlGroup(bool removeMembers, string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveAccessControlGroup");
			configurationApiItemWrapper.SetProperty("RemoveMembers", removeMembers.ToString());
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveAccessControlGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlGroupFolder", "Unable to call method 'RemoveAccessControlGroup':" + ex.Message);
		}
		return null;
	}

	public RemoveAccessControlGroupServerTask RemoveAccessControlGroup()
	{
		try
		{
			return new RemoveAccessControlGroupServerTask(base.Wrapper.ExecuteMethod("RemoveAccessControlGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlGroupFolder", "Unable to call method 'RemoveAccessControlGroup':" + ex.Message);
		}
		return null;
	}
}
