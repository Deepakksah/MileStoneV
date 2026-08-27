using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class SpeakerFolderItem : Item
{
	private ISpeakerGroupInfo _group;

	private ISystemInfo _systemInfoEx;

	public SpeakerFolderItem(ISystemInfo systemInfoEx, Guid parentId, ISpeakerGroupInfo group, string name)
		: base(systemInfoEx.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfoEx = systemInfoEx;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		ISpeakerGroupInfo[] speakerGroups = _group.SpeakerGroups;
		foreach (ISpeakerGroupInfo speakerGroupInfo in speakerGroups)
		{
			if (speakerGroupInfo.SpeakerGroups.Length != 0 || speakerGroupInfo.Speakers.Length != 0)
			{
				list.Add(new SpeakerFolderItem(_systemInfoEx, _group.GroupId, speakerGroupInfo, speakerGroupInfo.Name));
			}
		}
		Guid[] speakers = _group.Speakers;
		foreach (Guid id in speakers)
		{
			Speaker speaker = _systemInfoEx.Speakers[id];
			list.Add(new DeviceItem(speaker.Recorder.ServerId, speaker.Recorder.Guid, speaker.Guid, speaker.Name, Kind.Speaker, null, null, null, speaker));
		}
		return list;
	}
}
