using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class ApplicationSecurityInfo
{
	[DataMember]
	public SmartClientSecurityInfo SmartClientSecurity { get; set; }
}
