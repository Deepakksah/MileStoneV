using System;
using System.Collections.Generic;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Platform;

public class DeviceItem : Item
{
	private PositioningInformation _posInfo;

	private Dictionary<string, string> _authorization;

	public override Dictionary<string, string> Authorization
	{
		get
		{
			if (_authorization != null)
			{
				return _authorization;
			}
			_authorization = new Dictionary<string, string>();
			if (FQID.Kind == Kind.Output)
			{
				ISystemInfo systemInfo = ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).FindSystemInfo(base.FQID.ServerId, populate: true);
				if (systemInfo.OutputDevices.ContainsKey(FQID.ObjectId))
				{
					OutputDevice outputDevice = systemInfo.OutputDevices[FQID.ObjectId];
					_authorization.Add("Activate", (outputDevice != null && outputDevice.CanActivate) ? "Yes" : "No");
				}
			}
			else if (FQID.Kind == Kind.TriggerEvent)
			{
				try
				{
					SecurityAccess.CheckPermission(this, "Trigger", FQID.ServerId);
					_authorization["Trigger"] = "Yes";
				}
				catch (Exception)
				{
					_authorization["Trigger"] = "No";
				}
			}
			return _authorization;
		}
	}

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

	public DeviceItem(ServerId serverId, Guid parentId, Guid id, string name, Guid itemKind, List<Item> children, List<Item> related, Dictionary<string, string> properties = null, IDevice device = null)
		: base(new FQID(serverId, parentId, id, FolderType.No, itemKind), name)
	{
		_children = children;
		_related = related;
		base.HasChildren = ((_children == null || _children.Count <= 0) ? HasChildren.No : HasChildren.Yes);
		base.HasRelated = ((_related == null) ? HasRelated.Maybe : ((_related.Count <= 0) ? HasRelated.No : HasRelated.Yes));
		if (itemKind == Kind.Preset)
		{
			base.FQID.ObjectIdString = name;
			base.FQID.ObjectId = Guid.Empty;
		}
		if (itemKind == Kind.Microphone && device is IMicrophone microphone)
		{
			ConfigurationAsync.AddProperties(device, microphone.MicrophoneAccess, this);
		}
		if (itemKind == Kind.Speaker && device is ISpeaker speaker)
		{
			ConfigurationAsync.AddProperties(device, speaker.SpeakerAccess, this);
			ConfigurationAsync.AddSpeakerProperties(speaker, this);
		}
		if (itemKind == Kind.Metadata && device is IMetadataDevice metadataDevice)
		{
			ConfigurationAsync.AddProperties(device, metadataDevice.MetadataAccess, this);
		}
		if (properties != null)
		{
			foreach (string key in properties.Keys)
			{
				base.Properties[key] = properties[key];
			}
		}
		if (serverId.ServerType == "BLK" && (itemKind == Kind.Microphone || itemKind == Kind.Speaker || itemKind == Kind.Metadata) && device != null)
		{
			base.FQID.ObjectIdString = device.DBExportName;
		}
		if (device != null)
		{
			_posInfo = device.PositioningInformation;
		}
	}

	public DeviceItem(ServerId serverId, Guid parentId, Guid id, string name, FolderType folderType, Guid itemKind, List<Item> children, List<Item> related)
		: base(new FQID(serverId, parentId, id, folderType, itemKind), name)
	{
		_children = children;
		_related = related;
		base.HasChildren = ((_children == null || _children.Count <= 0) ? HasChildren.No : HasChildren.Yes);
		base.HasRelated = ((_related == null) ? HasRelated.Maybe : ((_related.Count <= 0) ? HasRelated.No : HasRelated.Yes));
		if (itemKind == Kind.Preset)
		{
			base.FQID.ObjectIdString = name;
			base.FQID.ObjectId = Guid.Empty;
		}
	}

	public override Item GetParent()
	{
		ISystemInfo systemInfo = ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).FindSystemInfo(base.FQID.ServerId, populate: true);
		if (systemInfo.Recorders.ContainsKey(FQID.ParentId))
		{
			IRecorder recorder = systemInfo.Recorders[FQID.ParentId];
			if (recorder != null)
			{
				return new RecorderFolderByTypeItem(systemInfo, recorder, Guid.Empty);
			}
		}
		return FQID.ServerId.UserContext.Configuration.GetItem(FQID.ServerId, FQID.ParentId, Kind.Server);
	}

	public override DataSource GetDataSource()
	{
		if (base.FQID.Kind == Kind.Camera)
		{
			return new SequenceDataSource(this);
		}
		if (base.FQID.Kind == Kind.Microphone)
		{
			return new SequenceDataSource(this);
		}
		if (base.FQID.Kind == Kind.Speaker)
		{
			return new SequenceDataSource(this);
		}
		if (base.FQID.Kind == Kind.Metadata)
		{
			return new MetadataSource(this);
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
			_related = new List<Item>();
			foreach (ICamera camera in ((SDKConfiguration)FQID.ServerId.UserContext.Configuration).FindSystemInfo(base.FQID.ServerId, populate: true).Cameras)
			{
				bool flag = false;
				if (camera.Microphone != null && camera.Microphone.Guid == FQID.ObjectId)
				{
					flag = true;
				}
				if (camera.Speaker != null && camera.Speaker.Guid == FQID.ObjectId)
				{
					flag = true;
				}
				if (camera.RelatedMetadataDevices != null)
				{
					foreach (IMetadataDevice relatedMetadataDevice in camera.RelatedMetadataDevices)
					{
						if (relatedMetadataDevice.Guid == FQID.ObjectId)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					_related.Add(FQID.ServerId.UserContext.Configuration.GetItem(camera.Guid, Kind.Camera));
				}
			}
			base.HasRelated = ((_related.Count <= 0) ? HasRelated.No : HasRelated.Yes);
		}
		return _related;
	}
}
