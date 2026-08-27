using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class InputInfo : DeviceInfo
{
	[DataMember]
	public InputSecurityInfo InputSecurity { get; set; }
}
