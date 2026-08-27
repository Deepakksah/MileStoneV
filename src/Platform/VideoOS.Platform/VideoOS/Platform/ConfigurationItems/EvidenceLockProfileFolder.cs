using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class EvidenceLockProfileFolder : IConfigurationItem
{
	public ICollection<EvidenceLockProfile> EvidenceLockProfiles
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<EvidenceLockProfile> list = new List<EvidenceLockProfile>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "EvidenceLockProfile")
				{
					list.Add(new EvidenceLockProfile(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public EvidenceLockProfileFolder(ServerId serverId, ConfigurationItem evidenceLockProfileFolder)
		: base(serverId, evidenceLockProfileFolder)
	{
	}

	public EvidenceLockProfileFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public EvidenceLockProfileFolder(FQID fqid)
		: base(fqid)
	{
	}

	public EvidenceLockProfileFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/EvidenceLockProfileFolder")
	{
	}

	public ServerTask AddEvidenceLockProfile(string name, Collection<string> retentionTimeOptions)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddEvidenceLockProfile");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("RetentionTimeOptions", retentionTimeOptions);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddEvidenceLockProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "EvidenceLockProfileFolder", "Unable to call method 'AddEvidenceLockProfile':" + ex.Message);
		}
		return null;
	}

	public AddEvidenceLockProfileServerTask AddEvidenceLockProfile()
	{
		try
		{
			return new AddEvidenceLockProfileServerTask(base.Wrapper.ExecuteMethod("AddEvidenceLockProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "EvidenceLockProfileFolder", "Unable to call method 'AddEvidenceLockProfile':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveEvidenceLockProfile(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveEvidenceLockProfile");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveEvidenceLockProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "EvidenceLockProfileFolder", "Unable to call method 'RemoveEvidenceLockProfile':" + ex.Message);
		}
		return null;
	}

	public RemoveEvidenceLockProfileServerTask RemoveEvidenceLockProfile()
	{
		try
		{
			return new RemoveEvidenceLockProfileServerTask(base.Wrapper.ExecuteMethod("RemoveEvidenceLockProfile"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "EvidenceLockProfileFolder", "Unable to call method 'RemoveEvidenceLockProfile':" + ex.Message);
		}
		return null;
	}
}
