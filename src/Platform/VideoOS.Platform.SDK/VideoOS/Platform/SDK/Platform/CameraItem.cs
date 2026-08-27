using System;
using System.Collections.Generic;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform;

public class CameraItem : ConfigItem
{
	private PositioningInformation _posInfo;

	public override PositioningInformation PositioningInformation
	{
		get
		{
			return _posInfo;
		}
		set
		{
		}
	}

	public CameraItem(ServerId serverId, ICamera camera)
		: base(new FQID(serverId, camera.Recorder.Guid, camera.Guid, FolderType.No, Kind.Camera), camera.Name)
	{
		_children = ConfigurationAsync.BuildPresets(serverId, camera, this);
		_related = ConfigurationAsync.BuildRelated(camera);
		ConfigurationAsync.AddProperties(camera, camera.CameraAccess, this);
		ConfigurationAsync.AddCameraProperties(camera, this);
		base.HasChildren = ((_children == null || _children.Count <= 0) ? HasChildren.No : HasChildren.Yes);
		base.HasRelated = ((_related == null || _related.Count <= 0) ? HasRelated.No : HasRelated.Yes);
		if (camera is CameraInFile cameraInFile)
		{
			base.FQID.ObjectIdString = cameraInFile.DBExportName;
		}
		_posInfo = camera.PositioningInformation;
	}

	public override Item GetParent()
	{
		ISystemInfo systemInfo = ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).FindSystemInfo(base.FQID.ServerId);
		if (systemInfo.Recorders.ContainsKey(FQID.ParentId))
		{
			IRecorder recorder = systemInfo.Recorders[FQID.ParentId];
			if (recorder != null)
			{
				return new RecorderFolderByTypeItem(systemInfo, recorder, Guid.Empty);
			}
		}
		return ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).GetItem(FQID.ServerId, FQID.ParentId, Kind.Server);
	}

	public override DataSource GetDataSource()
	{
		if (base.FQID.Kind == Kind.Camera)
		{
			return new SequenceDataSource(this);
		}
		return null;
	}

	public override List<Item> GetChildren()
	{
		if (_children == null)
		{
			return new List<Item>();
		}
		return _children;
	}

	public override List<Item> GetRelated()
	{
		if (_related == null)
		{
			return new List<Item>();
		}
		return _related;
	}

	public override ConfigItem AddChild(ConfigItem item)
	{
		try
		{
			Guid objectId = ((SDKConfiguration)item.FQID.ServerId.UserContext.Configuration).FindSystemInfo(item.FQID.ServerId, populate: true).LoginServer.CreatePreset(FQID.ObjectId, item.Name);
			item.FQID.ObjectId = objectId;
			_children.Add(item);
			return item;
		}
		catch (Exception ex)
		{
			throw new MIPException("Unable to add new preset:" + ex.Message);
		}
	}

	public override Item AddChild(Item item)
	{
		return AddChild(item.FQID, item.Name);
	}

	public override void AddRangeChild(ICollection<ConfigItem> items)
	{
		foreach (ConfigItem item in items)
		{
			AddChild(item);
		}
	}

	public override void AddRangeChild(ICollection<Item> items)
	{
		foreach (Item item in items)
		{
			AddChild(item);
		}
	}

	public override void RemoveChild(Item item)
	{
		try
		{
			if (!_children.Contains(item))
			{
				throw new MIPException("No child to remove");
			}
			((SDKConfiguration)FQID.ServerId.UserContext.Configuration).FindSystemInfo(FQID.ServerId, populate: true).LoginServer.DeletePreset(item.FQID.ObjectId, FQID.ObjectId);
			_children.Remove(item);
		}
		catch (Exception ex)
		{
			throw new MIPException("Unable to remove new preset:" + ex.Message);
		}
	}

	public override ConfigItem AddChild(FQID fqid, string name)
	{
		return AddChild(new PresetItem(this, fqid.ObjectId, name));
	}

	public override ConfigItem AddChild(string name, Guid kind, FolderType folderType)
	{
		if (kind != Kind.Preset)
		{
			throw new MIPException("Only Kind.Preset can be used");
		}
		if (folderType != FolderType.No)
		{
			throw new MIPException("Only FolderType.No can be used");
		}
		return AddChild(new PresetItem(this, Guid.NewGuid(), name));
	}

	public override void PropertiesModified()
	{
	}
}
