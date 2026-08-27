using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ConfigurationFaultType", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public enum ConfigurationFaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	InvalidAuthentication,
	[EnumMember]
	InvalidID,
	[EnumMember]
	ExistingID,
	[EnumMember]
	InvalidParameter
}
