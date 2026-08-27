using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform;

public class AllRSFolderItem : Item
{
	private ISystemInfo _recordingServerSystemInfo;

	private Guid _itemKind;

	private IRecorder _recorder;

	public AllRSFolderItem(ServerId serverId, string name, Guid itemKind, IRecorder recorder)
		: base(serverId, serverId.Id, itemKind, name, FolderType.SystemDefined, itemKind)
	{
		_recordingServerSystemInfo = ((SDKConfiguration)serverId.UserContext.Configuration).FindSystemInfo(serverId, populate: true);
		_recorder = recorder;
		_itemKind = itemKind;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		lock (_recorder.LockObject)
		{
			if (_itemKind == Kind.Camera)
			{
				foreach (ICamera camera in _recorder.Cameras)
				{
					list.Add(new CameraItem(_recorder.ServerId, camera));
				}
			}
			if (_itemKind == Kind.InputEvent)
			{
				foreach (ICamera camera2 in _recorder.Cameras)
				{
					List<Item> list2 = new List<Item>();
					foreach (UserDefinedEvent @event in camera2.Events)
					{
						if (@event.EventType != "Manual" && @event.EventType != "Output")
						{
							Item item = new DeviceItem(_recorder.ServerId, _recorder.Guid, @event.Guid, @event.Name, Kind.InputEvent, null, null);
							item.Properties["EventType"] = @event.EventType;
							item.Properties["SubType"] = @event.EventType;
							list2.Add(item);
						}
					}
					if (list2.Count > 0)
					{
						list.Add(new FolderItem(_recorder.ServerId, camera2.Guid, camera2.Name, FolderType.SystemDefined, Kind.Folder, list2));
					}
				}
				if (_recorder.ServerId.ServerType == "XPCORS")
				{
					foreach (InputEvent value in _recorder.Events.InputEvents.Values)
					{
						list.Add(new DeviceItem(_recorder.ServerId, _recorder.Guid, value.Guid, value.Name, Kind.InputEvent, null, null));
					}
				}
			}
			if (_itemKind == Kind.Output)
			{
				foreach (OutputDevice outputDevice in _recorder.OutputDevices)
				{
					list.Add(new DeviceItem(_recorder.ServerId, _recorder.Guid, outputDevice.Guid, outputDevice.Name, Kind.Output, null, null));
				}
			}
			if (_itemKind == Kind.Microphone)
			{
				foreach (IMicrophone microphone in _recorder.Microphones)
				{
					list.Add(new DeviceItem(_recorder.ServerId, _recorder.Guid, microphone.Guid, microphone.Name, Kind.Microphone, null, null, null, microphone));
				}
			}
			if (_itemKind == Kind.Metadata)
			{
				foreach (IMetadataDevice value2 in _recorder.MetadataDevices.Values)
				{
					list.Add(new DeviceItem(_recorder.ServerId, _recorder.Guid, value2.Guid, value2.Name, Kind.Metadata, null, null, ConfigurationAsync.BuildMetadataProperties(value2), value2));
				}
			}
			if (_itemKind == Kind.Speaker)
			{
				foreach (Speaker speaker in _recorder.Speakers)
				{
					list.Add(new DeviceItem(_recorder.ServerId, _recorder.Guid, speaker.Guid, speaker.Name, Kind.Speaker, null, null));
				}
			}
			if (_itemKind == Kind.Matrix)
			{
				foreach (MatrixMonitor matrixMonitor in _recorder.MatrixMonitors)
				{
					list.Add(new DeviceItem(_recorder.ServerId, _recorder.Guid, matrixMonitor.Guid, matrixMonitor.Name, Kind.Matrix, null, null));
				}
			}
			if (_itemKind == Kind.TriggerEvent)
			{
				foreach (UserDefinedEvent event2 in _recorder.Events)
				{
					if (event2.EventType == "Manual")
					{
						Item item2 = new DeviceItem(_recorder.ServerId, _recorder.Guid, event2.Guid, event2.Name, Kind.TriggerEvent, null, null);
						item2.Properties["EventType"] = event2.EventType;
						item2.Properties["SubType"] = event2.EventType;
						list.Add(item2);
					}
				}
			}
		}
		return list;
	}

	public override Item GetParent()
	{
		return new RecorderFolderByTypeItem(_recordingServerSystemInfo, _recorder, Guid.Empty);
	}
}
