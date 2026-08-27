using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class AllFolderItem : Item
{
	private ISystemInfo _systemInfo;

	private Guid _itemKind;

	public AllFolderItem(ISystemInfo systemInfoEx, string name, Guid itemKind)
		: base(systemInfoEx.ServerId, systemInfoEx.ServerId.Id, itemKind, name, FolderType.SystemDefined, itemKind)
	{
		_systemInfo = systemInfoEx;
		_itemKind = itemKind;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		if (_itemKind == Kind.Matrix)
		{
			foreach (MatrixMonitor matrixMonitor in _systemInfo.MatrixMonitors)
			{
				ServerId serverId = _systemInfo.ServerId;
				list.Add(new DeviceItem(serverId, serverId.Id, matrixMonitor.Guid, matrixMonitor.Name, Kind.Matrix, null, null));
			}
		}
		if (_itemKind == Kind.TriggerEvent)
		{
			foreach (UserDefinedEvent @event in _systemInfo.Events)
			{
				if (@event.EventType == "Manual")
				{
					IRecorder recorder = @event.Recorder;
					ServerId obj = ((recorder != null) ? recorder.ServerId : _systemInfo.ServerId);
					Item item = new DeviceItem(obj, obj.Id, @event.Guid, @event.Name, Kind.TriggerEvent, null, null);
					item.Properties["EventType"] = @event.EventType;
					item.Properties["SubType"] = @event.EventType;
					list.Add(item);
				}
			}
		}
		if (_itemKind == Kind.TriggerEvent)
		{
			HashSet<Guid> hashSet = new HashSet<Guid>
			{
				MessageManager.StartRecordingGuid,
				MessageManager.StopRecordingGuid,
				MessageManager.RequestPlayAudioGuid
			};
			if (_systemInfo.ConfigurationInfo != null)
			{
				if (_systemInfo.ConfigurationInfo.EventTypeGroups != null)
				{
					IEventTypeGroupInfo[] eventTypeGroups = _systemInfo.ConfigurationInfo.EventTypeGroups;
					for (int i = 0; i < eventTypeGroups.Length; i++)
					{
						Guid[] eventTypes = eventTypeGroups[i].EventTypes;
						foreach (Guid item2 in eventTypes)
						{
							hashSet.Add(item2);
						}
					}
				}
				if (_systemInfo.ConfigurationInfo.SystemEventTypes != null)
				{
					ISystemEventTypeInfo[] systemEventTypes = _systemInfo.ConfigurationInfo.SystemEventTypes;
					foreach (ISystemEventTypeInfo systemEventTypeInfo in systemEventTypes)
					{
						if (systemEventTypeInfo.EventSource == EventSource.External && !hashSet.Contains(systemEventTypeInfo.EventTypeId))
						{
							Item item3 = new DeviceItem(_systemInfo.ServerId, _systemInfo.Id, systemEventTypeInfo.EventTypeId, systemEventTypeInfo.Name, Kind.InputEvent, null, null);
							item3.Properties["EventType"] = "Generic";
							item3.Properties["SubType"] = "Generic";
							list.Add(item3);
						}
					}
				}
			}
		}
		if (_itemKind == Kind.TransactSource)
		{
			list.AddRange(Configuration.Instance.GetItemConfigurations(ConfigurationAsync.TransactPluginId, null, Kind.TransactSource));
		}
		return list;
	}

	public override Item GetParent()
	{
		return new ServerFolderByTypeItem(_systemInfo, Guid.Empty);
	}
}
