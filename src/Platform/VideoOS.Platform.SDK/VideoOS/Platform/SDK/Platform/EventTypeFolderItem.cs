using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class EventTypeFolderItem : Item
{
	private IEventTypeGroupInfo _group;

	private ISystemInfo _systemInfoEx;

	public EventTypeFolderItem(ISystemInfo systemInfoEx, Guid parentId, IEventTypeGroupInfo group, string name)
		: base(systemInfoEx.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfoEx = systemInfoEx;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		IEventTypeGroupInfo[] eventTypeGroups = _group.EventTypeGroups;
		foreach (IEventTypeGroupInfo eventTypeGroupInfo in eventTypeGroups)
		{
			if (eventTypeGroupInfo.EventTypeGroups.Length != 0 || eventTypeGroupInfo.EventTypes.Length != 0)
			{
				list.Add(new EventTypeFolderItem(_systemInfoEx, _group.GroupId, eventTypeGroupInfo, eventTypeGroupInfo.Name));
			}
		}
		Guid[] eventTypes = _group.EventTypes;
		foreach (Guid id in eventTypes)
		{
			if (_systemInfoEx.Events.ContainsKey(id))
			{
				UserDefinedEvent userDefinedEvent = _systemInfoEx.Events[id];
				IRecorder recorder = userDefinedEvent.Recorder;
				Item item = new DeviceItem((recorder != null) ? recorder.ServerId : _systemInfoEx.ServerId, recorder?.Guid ?? _systemInfoEx.Id, userDefinedEvent.Guid, userDefinedEvent.Name, Kind.TriggerEvent, null, null);
				item.Properties["EventType"] = userDefinedEvent.EventType;
				item.Properties["SubType"] = userDefinedEvent.EventType;
				list.Add(item);
			}
		}
		return list;
	}
}
