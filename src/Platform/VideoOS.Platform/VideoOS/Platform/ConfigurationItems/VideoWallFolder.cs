using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class VideoWallFolder : IConfigurationItem
{
	public ICollection<VideoWall> VideoWalls
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<VideoWall> list = new List<VideoWall>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "VideoWall")
				{
					list.Add(new VideoWall(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public VideoWallFolder(ServerId serverId, ConfigurationItem videoWallFolder)
		: base(serverId, videoWallFolder)
	{
	}

	public VideoWallFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public VideoWallFolder(FQID fqid)
		: base(fqid)
	{
	}

	public VideoWallFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/VideoWallFolder")
	{
	}

	public ServerTask AddVideoWall(string name, string description, bool statusText, string titleBar)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddVideoWall");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("StatusText", statusText.ToString());
			configurationApiItemWrapper.SetProperty("TitleBar", titleBar);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddVideoWall"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallFolder", "Unable to call method 'AddVideoWall':" + ex.Message);
		}
		return null;
	}

	public AddVideoWallServerTask AddVideoWall()
	{
		try
		{
			return new AddVideoWallServerTask(base.Wrapper.ExecuteMethod("AddVideoWall"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallFolder", "Unable to call method 'AddVideoWall':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveVideoWall(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveVideoWall");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveVideoWall"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallFolder", "Unable to call method 'RemoveVideoWall':" + ex.Message);
		}
		return null;
	}

	public RemoveVideoWallServerTask RemoveVideoWall()
	{
		try
		{
			return new RemoveVideoWallServerTask(base.Wrapper.ExecuteMethod("RemoveVideoWall"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallFolder", "Unable to call method 'RemoveVideoWall':" + ex.Message);
		}
		return null;
	}
}
