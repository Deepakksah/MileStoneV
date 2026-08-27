using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[Serializable]
[DataContract(Name = "RestrictedMediaLive", Namespace = "urn:milestone-systems")]
public class RestrictedMediaLive
{
	[DataMember]
	public FQID Device { get; set; }

	[DataMember]
	public string UserName { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public DateTime StartTime { get; set; }

	public Item GetDeviceItem()
	{
		return GetDeviceItem(Device.ObjectId, Kind.Camera) ?? GetDeviceItem(Device.ObjectId, Kind.Microphone) ?? GetDeviceItem(Device.ObjectId, Kind.Speaker) ?? GetDeviceItem(Device.ObjectId, Kind.Metadata);
	}

	private Item GetDeviceItem(Guid id, Guid kind)
	{
		if (Device.ServerId.UserContext != null)
		{
			return Device.ServerId.UserContext.Configuration.GetItem(id, kind);
		}
		return Configuration.Instance.GetItem(id, kind);
	}
}
