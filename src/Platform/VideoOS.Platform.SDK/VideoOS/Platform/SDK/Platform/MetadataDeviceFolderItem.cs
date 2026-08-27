using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class MetadataDeviceFolderItem : Item
{
	private readonly IMetadataDeviceGroupInfo _group;

	private readonly ISystemInfo _systemInfo;

	public MetadataDeviceFolderItem(ISystemInfo systemInfo, Guid parentId, IMetadataDeviceGroupInfo group, string name)
		: base(systemInfo.ServerId, parentId, group.GroupId, name, FolderType.UserDefined, Kind.Folder)
	{
		_group = group;
		_systemInfo = systemInfo;
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		IEnumerable<MetadataDeviceFolderItem> collection = from @group in _group.MetadataDeviceGroups
			where @group.MetadataDeviceGroups.Length != 0 || @group.MetadataDevices.Length != 0
			select new MetadataDeviceFolderItem(_systemInfo, _group.GroupId, @group, @group.Name);
		IEnumerable<DeviceItem> collection2 = from id in _group.MetadataDevices
			where _systemInfo.MetadataDevices.ContainsKey(id)
			let device = _systemInfo.MetadataDevices[id]
			select new DeviceItem(device.Recorder.ServerId, device.Recorder.Guid, device.Guid, device.Name, Kind.Metadata, null, null, ConfigurationAsync.BuildMetadataProperties(device), device);
		list.AddRange(collection);
		list.AddRange(collection2);
		return list;
	}
}
