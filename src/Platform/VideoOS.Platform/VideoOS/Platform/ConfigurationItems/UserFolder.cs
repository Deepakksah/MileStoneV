using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class UserFolder : IConfigurationItem
{
	public ICollection<User> Users
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<User> list = new List<User>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "User")
				{
					list.Add(new User(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public UserFolder(ServerId serverId, ConfigurationItem userFolder)
		: base(serverId, userFolder)
	{
	}

	public UserFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public UserFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddRoleMember(string sid)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRoleMember");
			configurationApiItemWrapper.SetProperty("Sid", sid);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRoleMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserFolder", "Unable to call method 'AddRoleMember':" + ex.Message);
		}
		return null;
	}

	public AddRoleMemberServerTask AddRoleMember()
	{
		try
		{
			return new AddRoleMemberServerTask(base.Wrapper.ExecuteMethod("AddRoleMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserFolder", "Unable to call method 'AddRoleMember':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveRoleMember(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveRoleMember");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveRoleMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserFolder", "Unable to call method 'RemoveRoleMember':" + ex.Message);
		}
		return null;
	}

	public RemoveRoleMemberServerTask RemoveRoleMember()
	{
		try
		{
			return new RemoveRoleMemberServerTask(base.Wrapper.ExecuteMethod("RemoveRoleMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "UserFolder", "Unable to call method 'RemoveRoleMember':" + ex.Message);
		}
		return null;
	}
}
