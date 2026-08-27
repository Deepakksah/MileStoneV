using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class MicrophoneFolderItem : Item
{
	private IMicrophoneGroupInfo _group;

	private ISystemInfo _systemInfoEx;

	public MicrophoneFolderItem(ISystemInfo systemInfoEx, Guid parentId, IMicrophoneGroupInfo group, string name)
		: base(systemInfoEx.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfoEx = systemInfoEx;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		IMicrophoneGroupInfo[] microphoneGroups = _group.MicrophoneGroups;
		foreach (IMicrophoneGroupInfo microphoneGroupInfo in microphoneGroups)
		{
			if (microphoneGroupInfo.MicrophoneGroups.Length != 0 || microphoneGroupInfo.Microphones.Length != 0)
			{
				list.Add(new MicrophoneFolderItem(_systemInfoEx, _group.GroupId, microphoneGroupInfo, microphoneGroupInfo.Name));
			}
		}
		Guid[] microphones = _group.Microphones;
		foreach (Guid id in microphones)
		{
			if (_systemInfoEx.Microphones.ContainsKey(id))
			{
				IMicrophone microphone = _systemInfoEx.Microphones[id];
				list.Add(new DeviceItem(microphone.Recorder.ServerId, microphone.Recorder.Guid, microphone.Guid, microphone.Name, Kind.Microphone, null, null, null, microphone));
			}
		}
		return list;
	}
}
