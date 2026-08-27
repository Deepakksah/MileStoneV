using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Resources;

namespace VideoOS.Platform.SDK.Platform;

public class ServerFolderByGroupItem : Item
{
	private ISystemInfo _sysInfo;

	private Guid _filterKind;

	public ServerFolderByGroupItem(ISystemInfo systemInfo, Guid filterKind)
		: base(systemInfo.ServerId, Guid.Empty, systemInfo.Id, systemInfo.Name, FolderType.UserDefined, Kind.Server)
	{
		_sysInfo = systemInfo;
		_filterKind = filterKind;
		base.Properties["URL"] = systemInfo.Url;
		base.Properties["Address"] = new UriBuilder(systemInfo.Url).Host;
		base.Properties["Port"] = systemInfo.Port.ToString() ?? "";
		base.Properties["Guid"] = systemInfo.Id.ToString();
		base.Properties["MacAddress"] = systemInfo.MacAddress ?? "";
		foreach (string key in systemInfo.Properties.Keys)
		{
			base.Properties[key] = systemInfo.Properties[key];
			if (key == "ServerType")
			{
				base.Properties["ProductCode"] = systemInfo.Properties[key];
			}
		}
	}

	public override List<Item> GetChildren()
	{
		_sysInfo.Populate();
		List<Item> list = new List<Item>();
		if (!_sysInfo.Populated)
		{
			return list;
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Camera)
		{
			Item item = BuildCameraGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.InputEvent)
		{
			Item item = BuildInputGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Output)
		{
			Item item = BuildOutputGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Microphone)
		{
			Item item = BuildMicrophoneGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Metadata)
		{
			Item item = BuildMetadataDeviceGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.TriggerEvent)
		{
			Item item = BuildEventGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		if (_filterKind == Guid.Empty || _filterKind == Kind.Speaker)
		{
			Item item = BuildSpeakerGroup(_sysInfo);
			if (item != null)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static Item BuildCameraGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			ICameraGroupInfo[] cameraGroups = systemInfo.ConfigurationInfo.CameraGroups;
			foreach (ICameraGroupInfo cameraGroupInfo in cameraGroups)
			{
				list.Add(new CameraFolderItem(systemInfo, Kind.Camera, cameraGroupInfo, cameraGroupInfo.Name));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.Camera, Strings.CameraGroups, Kind.Camera, list);
			}
		}
		catch
		{
		}
		return null;
	}

	public static Item BuildInputGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			IInputGroupInfo[] inputGroups = systemInfo.ConfigurationInfo.InputGroups;
			foreach (IInputGroupInfo inputGroupInfo in inputGroups)
			{
				list.Add(new InputFolderItem(systemInfo, Kind.InputEvent, inputGroupInfo, inputGroupInfo.Name));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.InputEvent, Strings.InputGroups, Kind.InputEvent, list);
			}
		}
		catch
		{
		}
		return null;
	}

	public static Item BuildOutputGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			IOutputGroupInfo[] outputGroups = systemInfo.ConfigurationInfo.OutputGroups;
			foreach (IOutputGroupInfo outputGroupInfo in outputGroups)
			{
				list.Add(new OutputFolderItem(systemInfo, Kind.Output, outputGroupInfo, outputGroupInfo.Name));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.Output, Strings.OutputGroups, Kind.Output, list);
			}
		}
		catch
		{
		}
		return null;
	}

	public static Item BuildMicrophoneGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			IMicrophoneGroupInfo[] microphoneGroups = systemInfo.ConfigurationInfo.MicrophoneGroups;
			foreach (IMicrophoneGroupInfo microphoneGroupInfo in microphoneGroups)
			{
				list.Add(new MicrophoneFolderItem(systemInfo, Kind.Microphone, microphoneGroupInfo, microphoneGroupInfo.Name));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.Microphone, Strings.MicrophoneGroups, Kind.Microphone, list);
			}
		}
		catch
		{
		}
		return null;
	}

	public static Item BuildMetadataDeviceGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			if (systemInfo.ConfigurationInfo.MetadataDeviceGroups == null)
			{
				return null;
			}
			IMetadataDeviceGroupInfo[] metadataDeviceGroups = systemInfo.ConfigurationInfo.MetadataDeviceGroups;
			foreach (IMetadataDeviceGroupInfo metadataDeviceGroupInfo in metadataDeviceGroups)
			{
				list.Add(new MetadataDeviceFolderItem(systemInfo, Kind.Metadata, metadataDeviceGroupInfo, metadataDeviceGroupInfo.Name));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.Metadata, Strings.MetadataDeviceGroups, Kind.Metadata, list);
			}
		}
		catch
		{
		}
		return null;
	}

	public static Item BuildSpeakerGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			ISpeakerGroupInfo[] speakerGroups = systemInfo.ConfigurationInfo.SpeakerGroups;
			foreach (ISpeakerGroupInfo speakerGroupInfo in speakerGroups)
			{
				list.Add(new SpeakerFolderItem(systemInfo, Kind.Speaker, speakerGroupInfo, speakerGroupInfo.Name));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.Speaker, Strings.SpeakerGroups, Kind.Speaker, list);
			}
		}
		catch
		{
		}
		return null;
	}

	public static Item BuildEventGroup(ISystemInfo systemInfo)
	{
		try
		{
			List<Item> list = new List<Item>();
			HashSet<Guid> hashSet = new HashSet<Guid>();
			hashSet.Add(MessageManager.StartRecordingGuid);
			hashSet.Add(MessageManager.StopRecordingGuid);
			hashSet.Add(MessageManager.RequestPlayAudioGuid);
			IEventTypeGroupInfo[] eventTypeGroups = systemInfo.ConfigurationInfo.EventTypeGroups;
			foreach (IEventTypeGroupInfo eventTypeGroupInfo in eventTypeGroups)
			{
				list.Add(new EventTypeFolderItem(systemInfo, Kind.TriggerEvent, eventTypeGroupInfo, eventTypeGroupInfo.Name));
				Guid[] eventTypes = eventTypeGroupInfo.EventTypes;
				foreach (Guid item in eventTypes)
				{
					hashSet.Add(item);
				}
			}
			List<Item> list2 = new List<Item>();
			ISystemEventTypeInfo[] systemEventTypes = systemInfo.ConfigurationInfo.SystemEventTypes;
			foreach (ISystemEventTypeInfo systemEventTypeInfo in systemEventTypes)
			{
				if (systemEventTypeInfo.EventSource == EventSource.External && !hashSet.Contains(systemEventTypeInfo.EventTypeId))
				{
					Item item2 = new DeviceItem(systemInfo.ServerId, systemInfo.Id, systemEventTypeInfo.EventTypeId, systemEventTypeInfo.Name, Kind.InputEvent, null, null);
					item2.Properties["EventType"] = "Generic";
					item2.Properties["SubType"] = "Generic";
					list2.Add(item2);
				}
			}
			if (list2.Count > 0)
			{
				list.Add(new FolderItem(systemInfo.ServerId, Kind.InputEvent, Strings.GenericEvents, Kind.InputEvent, list2));
			}
			if (list.Count > 0)
			{
				return new FolderItem(systemInfo.ServerId, Kind.TriggerEvent, Strings.EventGroups, Kind.TriggerEvent, list);
			}
		}
		catch
		{
		}
		return null;
	}
}
