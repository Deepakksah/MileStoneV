using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LprMatchListFolder : IConfigurationItem
{
	public ICollection<LprMatchList> LprMatchLists
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LprMatchList> list = new List<LprMatchList>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LprMatchList")
				{
					list.Add(new LprMatchList(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public LprMatchListFolder(ServerId serverId, ConfigurationItem lprMatchListFolder)
		: base(serverId, lprMatchListFolder)
	{
	}

	public LprMatchListFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LprMatchListFolder(FQID fqid)
		: base(fqid)
	{
	}

	public LprMatchListFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/LprMatchListFolder")
	{
	}

	public ServerTask MethodIdAddLprMatchList(string name)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdAddLprMatchList");
			configurationApiItemWrapper.SetProperty("Name", name);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdAddLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchListFolder", "Unable to call method 'MethodIdAddLprMatchList':" + ex.Message);
		}
		return null;
	}

	public MethodIdAddLprMatchListServerTask MethodIdAddLprMatchList()
	{
		try
		{
			return new MethodIdAddLprMatchListServerTask(base.Wrapper.ExecuteMethod("MethodIdAddLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchListFolder", "Unable to call method 'MethodIdAddLprMatchList':" + ex.Message);
		}
		return null;
	}

	public ServerTask MethodIdRemoveLprMatchList(string id)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MethodIdRemoveLprMatchList");
			configurationApiItemWrapper.SetProperty("Id", id);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MethodIdRemoveLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchListFolder", "Unable to call method 'MethodIdRemoveLprMatchList':" + ex.Message);
		}
		return null;
	}

	public MethodIdRemoveLprMatchListServerTask MethodIdRemoveLprMatchList()
	{
		try
		{
			return new MethodIdRemoveLprMatchListServerTask(base.Wrapper.ExecuteMethod("MethodIdRemoveLprMatchList"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LprMatchListFolder", "Unable to call method 'MethodIdRemoveLprMatchList':" + ex.Message);
		}
		return null;
	}
}
