using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class MetadataDeviceGroupInfo : GroupInfo
{
	[DataMember]
	public MetadataDeviceGroupInfo[] MetadataDeviceGroups { get; set; }

	[DataMember]
	public Guid[] MetadataDevices { get; set; }
}
