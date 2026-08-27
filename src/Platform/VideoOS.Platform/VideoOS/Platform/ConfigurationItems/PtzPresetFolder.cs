using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PtzPresetFolder : IConfigurationItem
{
	public ICollection<PtzPreset> PtzPresets
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<PtzPreset> list = new List<PtzPreset>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "PtzPreset")
				{
					list.Add(new PtzPreset(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public PtzPresetFolder(ServerId serverId, ConfigurationItem ptzPresetFolder)
		: base(serverId, ptzPresetFolder)
	{
	}

	public PtzPresetFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PtzPresetFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddPtzPreset(string name, string description, double pan, double tilt, double zoom)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddPtzPreset");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("Pan", pan.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("Tilt", tilt.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("Zoom", zoom.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddPtzPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'AddPtzPreset':" + ex.Message);
		}
		return null;
	}

	public AddPtzPresetServerTask AddPtzPreset()
	{
		try
		{
			return new AddPtzPresetServerTask(base.Wrapper.ExecuteMethod("AddPtzPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'AddPtzPreset':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemovePtzPreset(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemovePtzPreset");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemovePtzPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'RemovePtzPreset':" + ex.Message);
		}
		return null;
	}

	public RemovePtzPresetServerTask RemovePtzPreset()
	{
		try
		{
			return new RemovePtzPresetServerTask(base.Wrapper.ExecuteMethod("RemovePtzPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'RemovePtzPreset':" + ex.Message);
		}
		return null;
	}

	public ServerTask DefaultPtzPreset(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("DefaultPtzPreset");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("DefaultPtzPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'DefaultPtzPreset':" + ex.Message);
		}
		return null;
	}

	public DefaultPtzPresetServerTask DefaultPtzPreset()
	{
		try
		{
			return new DefaultPtzPresetServerTask(base.Wrapper.ExecuteMethod("DefaultPtzPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'DefaultPtzPreset':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask GetDevicePresets(int sessionDataId)
	{
		try
		{
			return new GetDevicePresetsServerTask(base.Wrapper.ExecuteMethod("GetDevicePresets", new Property[1]
			{
				new Property
				{
					Key = "SessionDataId",
					Value = sessionDataId.ToString(CultureInfo.InvariantCulture)
				}
			}));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'GetDevicePresets':" + ex.Message);
		}
		return null;
	}

	public GetDevicePresetsServerTask GetDevicePresets()
	{
		try
		{
			return new GetDevicePresetsServerTask(base.Wrapper.ExecuteMethod("GetDevicePresets", new Property[1]
			{
				new Property
				{
					Key = "SessionDataId",
					Value = 0.ToString(CultureInfo.InvariantCulture)
				}
			}));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPresetFolder", "Unable to call method 'GetDevicePresets':" + ex.Message);
		}
		return null;
	}
}
