using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
[GeneratedCode("CreateLogUserEnumerations build task", "0.0.0.0")]
public enum SourceType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	External,
	[EnumMember]
	System,
	[EnumMember]
	Server,
	[EnumMember]
	Recorder,
	[EnumMember]
	Device,
	[EnumMember]
	Database,
	[EnumMember]
	Hardware,
	[EnumMember]
	Failover,
	[EnumMember]
	IdentityProvider,
	[EnumMember]
	Role
}
