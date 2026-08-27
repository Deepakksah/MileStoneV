using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ArchiveStorageFolder : IConfigurationItem
{
	public ICollection<ArchiveStorage> ArchiveStorages
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ArchiveStorage> list = new List<ArchiveStorage>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ArchiveStorage")
				{
					list.Add(new ArchiveStorage(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ArchiveStorageFolder(ServerId serverId, ConfigurationItem archiveStorageFolder)
		: base(serverId, archiveStorageFolder)
	{
	}

	public ArchiveStorageFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ArchiveStorageFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddArchiveStorage(string name, string description, string diskPath, double targetFramerate, bool framerateReductionEnabled, int retainMinutes, int maxSize)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddArchiveStorage");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DiskPath", diskPath);
			configurationApiItemWrapper.SetProperty("TargetFramerate", targetFramerate.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("FramerateReductionEnabled", framerateReductionEnabled.ToString());
			configurationApiItemWrapper.SetProperty("RetainMinutes", retainMinutes.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("MaxSize", maxSize.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorageFolder", "Unable to call method 'AddArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public AddArchiveStorageServerTask AddArchiveStorage()
	{
		try
		{
			return new AddArchiveStorageServerTask(base.Wrapper.ExecuteMethod("AddArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorageFolder", "Unable to call method 'AddArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveArchiveStorage(string itemSelection, bool deleteIfOffline)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveArchiveStorage");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			configurationApiItemWrapper.SetProperty("DeleteIfOffline", deleteIfOffline.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorageFolder", "Unable to call method 'RemoveArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public RemoveArchiveStorageServerTask RemoveArchiveStorage()
	{
		try
		{
			return new RemoveArchiveStorageServerTask(base.Wrapper.ExecuteMethod("RemoveArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorageFolder", "Unable to call method 'RemoveArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveArchiveStorage(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveArchiveStorage");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorageFolder", "Unable to call method 'RemoveArchiveStorage':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddArchiveStorage(string name, string description, string diskPath, double targetFramerate, int retainMinutes, int maxSize)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddArchiveStorage");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DiskPath", diskPath);
			configurationApiItemWrapper.SetProperty("TargetFramerate", targetFramerate.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("RetainMinutes", retainMinutes.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("MaxSize", maxSize.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddArchiveStorage"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ArchiveStorageFolder", "Unable to call method 'AddArchiveStorage':" + ex.Message);
		}
		return null;
	}
}
