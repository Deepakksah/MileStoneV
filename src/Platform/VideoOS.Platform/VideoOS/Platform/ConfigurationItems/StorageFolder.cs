using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StorageFolder : IConfigurationItem
{
	public ICollection<Storage> Storages
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Storage> list = new List<Storage>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Storage")
				{
					list.Add(new Storage(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public StorageFolder(ServerId serverId, ConfigurationItem storageFolder)
		: base(serverId, storageFolder)
	{
	}

	public StorageFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StorageFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddStorage(string name, string description, string diskPath, bool signing, int retainMinutes, int maxSize)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddStorage");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DiskPath", diskPath);
			configurationApiItemWrapper.SetProperty("Signing", signing.ToString());
			configurationApiItemWrapper.SetProperty("RetainMinutes", retainMinutes.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("MaxSize", maxSize.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StorageFolder", "Unable to call method 'AddStorage':" + ex.Message);
		}
		return null;
	}

	public AddStorageServerTask AddStorage()
	{
		try
		{
			return new AddStorageServerTask(base.Wrapper.ExecuteMethod("AddStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StorageFolder", "Unable to call method 'AddStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveStorage(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveStorage");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StorageFolder", "Unable to call method 'RemoveStorage':" + ex.Message);
		}
		return null;
	}

	public RemoveStorageServerTask RemoveStorage()
	{
		try
		{
			return new RemoveStorageServerTask(base.Wrapper.ExecuteMethod("RemoveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StorageFolder", "Unable to call method 'RemoveStorage':" + ex.Message);
		}
		return null;
	}
}
