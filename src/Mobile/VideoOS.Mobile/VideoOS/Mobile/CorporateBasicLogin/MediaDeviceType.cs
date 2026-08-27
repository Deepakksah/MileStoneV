using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MediaDeviceType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum MediaDeviceType
{
	[EnumMember]
	Camera,
	[EnumMember]
	Microphone,
	[EnumMember]
	Speaker
}
