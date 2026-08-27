using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Util.VideoWall;

public class VideoWallMessageHandler
{
	private List<object> messageRegistrations = new List<object>();

	public VideoWallMessageHandler()
	{
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallPresetActivateHandler, new MessageIdFilter("Control.VideoWallPresetActivateCommand")));
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallSetLayoutAndDevicesHandler, new MessageIdFilter("Control.VideoWallSetLayoutAndCamerasCommand")));
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallShowTextHandler, new MessageIdFilter("Control.VideoWallShowTextCommand")));
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallSetLayoutHandler, new MessageIdFilter("Control.VideoWallSetLayoutCommand")));
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallApplyXmlHandler, new MessageIdFilter("Control.VideoWallApplyXmlCommand")));
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallRemoveDevicesHandler, new MessageIdFilter("Control.VideoWallRemoveCamerasCommand")));
		messageRegistrations.Add(EnvironmentManager.Instance.RegisterReceiver(VideoWallSetDevicesHandler, new MessageIdFilter("Control.VideoWallSetCamerasCommand")));
	}

	public void Close()
	{
		foreach (object messageRegistration in messageRegistrations)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(messageRegistration);
		}
		messageRegistrations.Clear();
	}

	private object VideoWallPresetActivateHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallPresetActivateCommand").Send();
		if (dest == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallPresetActivateCommand needs the destination set to a MonitorPresetFQTD");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		new VideoWallPreset(dest.ServerId, configurationApiItemWrapper.ConfigurationItem).Activate();
		return null;
	}

	private object VideoWallSetLayoutAndDevicesHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallSetLayoutAndCamerasCommand").Send();
		if (!(message.Data is VideoWallSetLayoutAndCamerasCommandData videoWallSetLayoutAndCamerasCommandData))
		{
			return null;
		}
		if (dest == null || videoWallSetLayoutAndCamerasCommandData.CameraFQIDList == null || videoWallSetLayoutAndCamerasCommandData.LayoutFQID == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallSetLayoutAndCamerasCommand data cannot contain null fields the LayoutFQID, CameraFQIDList, or SendMessage destination");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		Monitor monitor = new Monitor(dest.ServerId, configurationApiItemWrapper.ConfigurationItem);
		Collection<string> collection = new Collection<string>();
		foreach (FQID cameraFQID in videoWallSetLayoutAndCamerasCommandData.CameraFQIDList)
		{
			collection.Add(new ConfigurationItemPath(cameraFQID).Path);
		}
		ApplyCamerasAndLayoutServerTask applyCamerasAndLayoutServerTask = monitor.ApplyCamerasAndLayout();
		applyCamerasAndLayoutServerTask.LayoutPath = new ConfigurationItemPath(videoWallSetLayoutAndCamerasCommandData.LayoutFQID).Path;
		applyCamerasAndLayoutServerTask.BeginPosition = videoWallSetLayoutAndCamerasCommandData.Position;
		applyCamerasAndLayoutServerTask.CameraPaths = collection;
		ServerTask serverTask = applyCamerasAndLayoutServerTask.Execute();
		if (serverTask.State != StateEnum.Success)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallSetLayoutAndDevicesHandler", "Unable to set layout and cameras:" + serverTask.ErrorCode);
		}
		return null;
	}

	private object VideoWallSetDevicesHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallSetCamerasCommand").Send();
		if (!(message.Data is VideoWallSetCamerasCommandData videoWallSetCamerasCommandData))
		{
			return null;
		}
		if (dest == null || videoWallSetCamerasCommandData.CameraFQIDList == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallSetCamerasCommandData data cannot contain null fields the CameraFQIDList, or SendMessage destination");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		Monitor monitor = new Monitor(dest.ServerId, configurationApiItemWrapper.ConfigurationItem);
		Collection<string> collection = new Collection<string>();
		foreach (FQID cameraFQID in videoWallSetCamerasCommandData.CameraFQIDList)
		{
			collection.Add(new ConfigurationItemPath(cameraFQID).Path);
		}
		ApplyCamerasServerTask applyCamerasServerTask = monitor.ApplyCameras();
		applyCamerasServerTask.BeginPosition = videoWallSetCamerasCommandData.Position;
		applyCamerasServerTask.CameraPaths = collection;
		ServerTask serverTask = applyCamerasServerTask.Execute();
		if (serverTask.State != StateEnum.Success)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallSetLayoutAndDevicesHandler", "Unable to set layout and cameras:" + serverTask.ErrorCode);
		}
		return null;
	}

	private object VideoWallSetLayoutHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallSetLayoutCommand").Send();
		if (!(message.Data is VideoWallSetLayoutCommandData videoWallSetLayoutCommandData))
		{
			return null;
		}
		if (dest == null || videoWallSetLayoutCommandData.LayoutFQID == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallSetLayoutCommandData data cannot contain null fields the LayoutFQID or SendMessage destination");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		new Monitor(dest.ServerId, configurationApiItemWrapper.ConfigurationItem).ApplyLayout(new ConfigurationItemPath(videoWallSetLayoutCommandData.LayoutFQID).Path);
		return null;
	}

	private object VideoWallShowTextHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallShowTextCommand").Send();
		if (!(message.Data is VideoWallShowTextCommandData videoWallShowTextCommandData))
		{
			return null;
		}
		if (dest == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallShowTextCommand needs to be send with the destination of the monitorFQID");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		Monitor monitor = new Monitor(dest.ServerId, configurationApiItemWrapper.ConfigurationItem);
		string text = "";
		text = new ConfigurationItemPath(ConfigurationApiClassConverter.ConvertToItemType(Kind.Layout), Guid.Empty).Path;
		ApplyTextAndLayoutServerTask applyTextAndLayoutServerTask = monitor.ApplyTextAndLayout();
		applyTextAndLayoutServerTask.LayoutPath = text;
		applyTextAndLayoutServerTask.BeginPosition = videoWallShowTextCommandData.Position;
		applyTextAndLayoutServerTask.TextMessage = videoWallShowTextCommandData.Text;
		ServerTask serverTask = applyTextAndLayoutServerTask.Execute();
		if (serverTask.State != StateEnum.Success)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallShowTextHandler", "Unable to set text:" + serverTask.ErrorCode);
		}
		return null;
	}

	private object VideoWallRemoveDevicesHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallRemoveCamerasCommand").Send();
		if (!(message.Data is VideoWallRemoveCamerasCommandData videoWallRemoveCamerasCommandData))
		{
			return null;
		}
		if (dest == null || videoWallRemoveCamerasCommandData.CameraFQIDList == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallRemoveCamerasCommandData data cannot contain null fields the CameraFQIDList, or SendMessage destination");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		Monitor monitor = new Monitor(dest.ServerId, configurationApiItemWrapper.ConfigurationItem);
		Collection<string> collection = new Collection<string>();
		foreach (FQID cameraFQID in videoWallRemoveCamerasCommandData.CameraFQIDList)
		{
			collection.Add(new ConfigurationItemPath(cameraFQID).Path);
		}
		monitor.RemoveCameras(ConfigurationApiUtil.ToSeparatorString(collection), "");
		return null;
	}

	private object VideoWallApplyXmlHandler(Message message, FQID dest, FQID source)
	{
		new TrackEventData("Control.VideoWallApplyXmlCommand").Send();
		if (!(message.Data is VideoWallApplyXmlCommandData videoWallApplyXmlCommandData))
		{
			return null;
		}
		if (dest == null || videoWallApplyXmlCommandData.Xml == null)
		{
			EnvironmentManager.Instance.Log(error: true, "VideoWallMessageHandler", "VideoWallApplyXmlCommandData data cannot contain null fields the Xml, or SendMessage destination");
			return null;
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(dest.ServerId, new ConfigurationItemPath(dest).Path);
		ApplyViewItemsXmlServerTask applyViewItemsXmlServerTask = new Monitor(dest.ServerId, configurationApiItemWrapper.ConfigurationItem).ApplyViewItemsXml();
		if (applyViewItemsXmlServerTask == null)
		{
			return null;
		}
		applyViewItemsXmlServerTask.BeginPosition = videoWallApplyXmlCommandData.Position;
		applyViewItemsXmlServerTask.ViewItemsXml = videoWallApplyXmlCommandData.Xml;
		ServerTask serverTask = applyViewItemsXmlServerTask.Execute();
		if (serverTask.State != StateEnum.Success)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWallApplyXmlHandler", "Unable to set XML:" + serverTask.ErrorCode);
		}
		return null;
	}
}
