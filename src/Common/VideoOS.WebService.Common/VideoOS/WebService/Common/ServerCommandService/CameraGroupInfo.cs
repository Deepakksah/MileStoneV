using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class CameraGroupInfo : GroupInfo
{
	[DataMember]
	public CameraGroupInfo[] CameraGroups { get; set; }

	[DataMember]
	public Guid[] Cameras { get; set; }
}
