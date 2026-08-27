using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareFolder : IConfigurationItem
{
	public ICollection<Hardware> Hardwares
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Hardware> list = new List<Hardware>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Hardware")
				{
					list.Add(new Hardware(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public HardwareFolder(ServerId serverId, ConfigurationItem hardwareFolder)
		: base(serverId, hardwareFolder)
	{
	}

	public HardwareFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask DeleteHardware(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("DeleteHardware");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("DeleteHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "HardwareFolder", "Unable to call method 'DeleteHardware':" + ex.Message);
		}
		return null;
	}

	public DeleteHardwareServerTask DeleteHardware()
	{
		try
		{
			return new DeleteHardwareServerTask(base.Wrapper.ExecuteMethod("DeleteHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "HardwareFolder", "Unable to call method 'DeleteHardware':" + ex.Message);
		}
		return null;
	}
}
