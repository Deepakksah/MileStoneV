using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum MediaDeviceType
{
	[EnumMember]
	Camera,
	[EnumMember]
	Microphone,
	[EnumMember]
	Speaker
}
