using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum IntegrationType
{
	[EnumMember]
	Client,
	[EnumMember]
	Administration,
	[EnumMember]
	EventServer,
	[EnumMember]
	Standalone,
	[EnumMember]
	DeviceDriver
}
