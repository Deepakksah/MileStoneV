using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfileFolder : IConfigurationItem
{
	public ICollection<ClientProfile> ClientProfiles
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfile> list = new List<ClientProfile>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfile")
				{
					list.Add(new ClientProfile(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ClientProfileFolder(ServerId serverId, ConfigurationItem clientProfileFolder)
		: base(serverId, clientProfileFolder)
	{
	}

	public ClientProfileFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfileFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ClientProfileFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/ClientProfileFolder")
	{
	}

	public ServerTask AddClientProfile(string name, string description)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddClientProfile");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddClientProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClientProfileFolder", "Unable to call method 'AddClientProfile':" + ex.Message);
		}
		return null;
	}

	public AddClientProfileServerTask AddClientProfile()
	{
		try
		{
			return new AddClientProfileServerTask(base.Wrapper.ExecuteMethod("AddClientProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClientProfileFolder", "Unable to call method 'AddClientProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveClientProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveClientProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveClientProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClientProfileFolder", "Unable to call method 'RemoveClientProfile':" + ex.Message);
		}
		return null;
	}

	public RemoveClientProfileServerTask RemoveClientProfile()
	{
		try
		{
			return new RemoveClientProfileServerTask(base.Wrapper.ExecuteMethod("RemoveClientProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClientProfileFolder", "Unable to call method 'RemoveClientProfile':" + ex.Message);
		}
		return null;
	}
}
