using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PermissionType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public enum PermissionType
{
	[EnumMember]
	Granted,
	[EnumMember]
	Denied,
	[EnumMember]
	Default
}
