using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class CameraFolderItem : Item
{
	private ICameraGroupInfo _group;

	private ISystemInfo _systemInfoEx;

	public CameraFolderItem(ISystemInfo systemInfoEx, Guid parentId, ICameraGroupInfo group, string name)
		: base(systemInfoEx.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfoEx = systemInfoEx;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		ICameraGroupInfo[] cameraGroups = _group.CameraGroups;
		foreach (ICameraGroupInfo cameraGroupInfo in cameraGroups)
		{
			list.Add(new CameraFolderItem(_systemInfoEx, _group.GroupId, cameraGroupInfo, cameraGroupInfo.Name));
		}
		Guid[] cameras = _group.Cameras;
		foreach (Guid id in cameras)
		{
			ICamera camera = _systemInfoEx.Cameras[id];
			list.Add(new CameraItem(camera.Recorder.ServerId, camera));
		}
		return list;
	}
}
