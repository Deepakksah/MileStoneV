using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class BasicUserFolder : IConfigurationItem
{
	public ICollection<BasicUser> BasicUsers
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<BasicUser> list = new List<BasicUser>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "BasicUser")
				{
					list.Add(new BasicUser(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public BasicUserFolder(ServerId serverId, ConfigurationItem basicUserFolder)
		: base(serverId, basicUserFolder)
	{
	}

	public BasicUserFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public BasicUserFolder(FQID fqid)
		: base(fqid)
	{
	}

	public BasicUserFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/BasicUserFolder")
	{
	}

	public ServerTask AddBasicUser(string name, string description, bool canChangePassword, bool forcePasswordChange, SecureString password, string status)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddBasicUser");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("CanChangePassword", canChangePassword.ToString());
			configurationApiItemWrapper.SetProperty("ForcePasswordChange", forcePasswordChange.ToString());
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("Status", status);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUserFolder", "Unable to call method 'AddBasicUser':" + ex.Message);
		}
		return null;
	}

	public AddBasicUserServerTask AddBasicUser()
	{
		try
		{
			return new AddBasicUserServerTask(base.Wrapper.ExecuteMethod("AddBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUserFolder", "Unable to call method 'AddBasicUser':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveBasicUser(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveBasicUser");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUserFolder", "Unable to call method 'RemoveBasicUser':" + ex.Message);
		}
		return null;
	}

	public RemoveBasicUserServerTask RemoveBasicUser()
	{
		try
		{
			return new RemoveBasicUserServerTask(base.Wrapper.ExecuteMethod("RemoveBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUserFolder", "Unable to call method 'RemoveBasicUser':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddBasicUser(string name, string description, string password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddBasicUser");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUserFolder", "Unable to call method 'AddBasicUser':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddBasicUser(string name, string description, SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddBasicUser");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUserFolder", "Unable to call method 'AddBasicUser':" + ex.Message);
		}
		return null;
	}
}
