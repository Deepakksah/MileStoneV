using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[Serializable]
[DataContract(Name = "RestrictedMedia", Namespace = "urn:milestone-systems")]
public class RestrictedMedia
{
	[DataMember]
	public FQID FQID { get; private set; }

	[DataMember]
	public Collection<FQID> Devices { get; private set; }

	[DataMember]
	public string Header { get; private set; }

	[DataMember]
	public string Description { get; private set; }

	[DataMember]
	public string UserName { get; private set; }

	[DataMember]
	public DateTime LastModified { get; private set; }

	[DataMember]
	public DateTime StartTime { get; private set; }

	[DataMember]
	public DateTime EndTime { get; private set; }

	public RestrictedMedia(FQID restrictedMediaFQID, IEnumerable<FQID> devices, string header, string description, string userName, DateTime lastModified, DateTime startTime, DateTime endTime)
	{
		FQID = restrictedMediaFQID;
		Devices = new Collection<FQID>(devices.ToList());
		Header = header;
		Description = description;
		UserName = userName;
		LastModified = lastModified;
		StartTime = startTime;
		EndTime = endTime;
	}

	public Item GetDeviceItem()
	{
		return GetDeviceItem(FQID.ParentId, Kind.Camera) ?? GetDeviceItem(FQID.ParentId, Kind.Microphone) ?? GetDeviceItem(FQID.ParentId, Kind.Speaker) ?? GetDeviceItem(FQID.ParentId, Kind.Metadata);
	}

	private Item GetDeviceItem(Guid id, Guid kind)
	{
		if (FQID.ServerId.UserContext != null)
		{
			return FQID.ServerId.UserContext.Configuration.GetItem(id, kind);
		}
		return Configuration.Instance.GetItem(id, kind);
	}

	public static FQID MakeFQID(ServerId serverid, Guid objectId)
	{
		return new FQID(serverid, serverid.Id, objectId, FolderType.No, Kind.RestrictedMedia);
	}
}
