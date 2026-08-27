using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class OutputFolderItem : Item
{
	private IOutputGroupInfo _group;

	private ISystemInfo _systemInfoEx;

	public OutputFolderItem(ISystemInfo systemInfoEx, Guid parentId, IOutputGroupInfo group, string name)
		: base(systemInfoEx.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfoEx = systemInfoEx;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		IOutputGroupInfo[] outputGroups = _group.OutputGroups;
		foreach (IOutputGroupInfo outputGroupInfo in outputGroups)
		{
			if (outputGroupInfo.OutputGroups.Length != 0 || outputGroupInfo.Outputs.Length != 0)
			{
				list.Add(new OutputFolderItem(_systemInfoEx, _group.GroupId, outputGroupInfo, outputGroupInfo.Name));
			}
		}
		Guid[] outputs = _group.Outputs;
		foreach (Guid id in outputs)
		{
			if (_systemInfoEx.OutputDevices.ContainsKey(id))
			{
				OutputDevice outputDevice = _systemInfoEx.OutputDevices[id];
				list.Add(new DeviceItem(outputDevice.Recorder.ServerId, outputDevice.Recorder.Guid, outputDevice.Guid, outputDevice.Name, Kind.Output, null, null));
			}
		}
		return list;
	}
}
