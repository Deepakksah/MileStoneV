using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AccessControlUnitFolder : IConfigurationItem
{
	public ICollection<AccessControlUnit> AccessControlUnits
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<AccessControlUnit> list = new List<AccessControlUnit>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "AccessControlUnit")
				{
					list.Add(new AccessControlUnit(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public AccessControlUnitFolder(ServerId serverId, ConfigurationItem accessControlUnitFolder)
		: base(serverId, accessControlUnitFolder)
	{
	}

	public AccessControlUnitFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AccessControlUnitFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddAccessControlGroupMember(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAccessControlGroupMember");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAccessControlGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlUnitFolder", "Unable to call method 'AddAccessControlGroupMember':" + ex.Message);
		}
		return null;
	}

	public AddAccessControlGroupMemberServerTask AddAccessControlGroupMember()
	{
		try
		{
			return new AddAccessControlGroupMemberServerTask(base.Wrapper.ExecuteMethod("AddAccessControlGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlUnitFolder", "Unable to call method 'AddAccessControlGroupMember':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveAccessControlGroupMember(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveAccessControlGroupMember");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveAccessControlGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlUnitFolder", "Unable to call method 'RemoveAccessControlGroupMember':" + ex.Message);
		}
		return null;
	}

	public RemoveAccessControlGroupMemberServerTask RemoveAccessControlGroupMember()
	{
		try
		{
			return new RemoveAccessControlGroupMemberServerTask(base.Wrapper.ExecuteMethod("RemoveAccessControlGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlUnitFolder", "Unable to call method 'RemoveAccessControlGroupMember':" + ex.Message);
		}
		return null;
	}
}
