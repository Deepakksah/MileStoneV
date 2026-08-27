using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class VideoWallPresetFolder : IConfigurationItem
{
	public ICollection<VideoWallPreset> VideoWallPresets
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<VideoWallPreset> list = new List<VideoWallPreset>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "VideoWallPreset")
				{
					list.Add(new VideoWallPreset(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public VideoWallPresetFolder(ServerId serverId, ConfigurationItem videoWallPresetFolder)
		: base(serverId, videoWallPresetFolder)
	{
	}

	public VideoWallPresetFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public VideoWallPresetFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddVideoWallPreset(string name, string description)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddVideoWallPreset");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddVideoWallPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallPresetFolder", "Unable to call method 'AddVideoWallPreset':" + ex.Message);
		}
		return null;
	}

	public AddVideoWallPresetServerTask AddVideoWallPreset()
	{
		try
		{
			return new AddVideoWallPresetServerTask(base.Wrapper.ExecuteMethod("AddVideoWallPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallPresetFolder", "Unable to call method 'AddVideoWallPreset':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveVideoWallPreset(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveVideoWallPreset");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveVideoWallPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallPresetFolder", "Unable to call method 'RemoveVideoWallPreset':" + ex.Message);
		}
		return null;
	}

	public RemoveVideoWallPresetServerTask RemoveVideoWallPreset()
	{
		try
		{
			return new RemoveVideoWallPresetServerTask(base.Wrapper.ExecuteMethod("RemoveVideoWallPreset"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallPresetFolder", "Unable to call method 'RemoveVideoWallPreset':" + ex.Message);
		}
		return null;
	}
}
