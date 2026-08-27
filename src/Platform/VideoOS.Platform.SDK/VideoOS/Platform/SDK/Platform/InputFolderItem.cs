using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class InputFolderItem : Item
{
	private IInputGroupInfo _group;

	private ISystemInfo _systemInfoEx;

	public InputFolderItem(ISystemInfo systemInfoEx, Guid parentId, IInputGroupInfo group, string name)
		: base(systemInfoEx.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfoEx = systemInfoEx;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		IInputGroupInfo[] inputGroups = _group.InputGroups;
		foreach (IInputGroupInfo inputGroupInfo in inputGroups)
		{
			list.Add(new InputFolderItem(_systemInfoEx, _group.GroupId, inputGroupInfo, inputGroupInfo.Name));
		}
		Guid[] inputs = _group.Inputs;
		foreach (Guid id in inputs)
		{
			try
			{
				InputEvent inputEvent = _systemInfoEx.Events.InputEvent(id);
				if (inputEvent != null)
				{
					IRecorder recorder = _systemInfoEx.Recorders[inputEvent.RecorderGuid];
					if (recorder != null)
					{
						list.Add(new DeviceItem(recorder.ServerId, recorder.ServerId.Id, inputEvent.Guid, inputEvent.Name, Kind.InputEvent, null, null));
					}
				}
			}
			catch (Exception)
			{
			}
		}
		return list;
	}
}
