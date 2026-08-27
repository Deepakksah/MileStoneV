using System;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform;

public class PresetItem : ConfigItem
{
	private ConfigItem _cameraItem;

	public PresetItem(Item cameraItem, Guid id, string name)
		: base(new FQID(cameraItem.FQID.ServerId, cameraItem.FQID.ObjectId, id, FolderType.No, Kind.Preset), name)
	{
		_cameraItem = cameraItem as ConfigItem;
		_children = new List<Item>();
		_related = new List<Item>();
		base.HasChildren = HasChildren.No;
		base.HasRelated = HasRelated.No;
		base.FQID.ObjectIdString = name;
	}

	public PresetItem(ServerId serverId, Guid parentId, Guid id, string name)
		: base(new FQID(serverId, parentId, id, FolderType.No, Kind.Preset), name)
	{
		base.FQID.ObjectIdString = name;
	}

	public override Item GetParent()
	{
		return _cameraItem;
	}

	public override void PropertiesModified()
	{
		ISystemInfo systemInfo = ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).FindSystemInfo(FQID.ServerId, populate: true);
		systemInfo.LoginServer.UpdatePresetName(FQID.ObjectId, FQID.ParentId, Name);
		systemInfo.LoginServer.UpdatePresetPosition(FQID.ObjectId, FQID.ParentId);
	}
}
