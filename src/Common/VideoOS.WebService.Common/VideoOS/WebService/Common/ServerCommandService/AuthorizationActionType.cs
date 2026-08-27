using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum AuthorizationActionType
{
	[EnumMember]
	None,
	[EnumMember]
	LogOn
}
