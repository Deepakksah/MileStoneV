using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FilterType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public enum FilterType
{
	[EnumMember]
	SourceName,
	[EnumMember]
	User,
	[EnumMember]
	UserLocation,
	[EnumMember]
	RuleName,
	[EnumMember]
	ServiceName,
	[EnumMember]
	EventType,
	[EnumMember]
	LogLevel,
	[EnumMember]
	Category,
	[EnumMember]
	SourceType,
	[EnumMember]
	PermissionType
}
