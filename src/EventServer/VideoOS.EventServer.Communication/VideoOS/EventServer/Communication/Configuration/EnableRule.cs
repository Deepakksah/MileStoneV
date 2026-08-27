using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EnableRule", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public enum EnableRule
{
	[EnumMember]
	Always,
	[EnumMember]
	TimeProfile,
	[EnumMember]
	EventTriggered
}
