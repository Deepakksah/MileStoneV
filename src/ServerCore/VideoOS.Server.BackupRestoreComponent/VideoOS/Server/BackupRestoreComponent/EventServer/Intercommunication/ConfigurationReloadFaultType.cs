using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ConfigurationReloadFaultType", Namespace = "http://videoos.net/2/CentralServerConfigurationReloadCommandToken")]
public enum ConfigurationReloadFaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	InvalidToken,
	[EnumMember]
	Unauthorized,
	[EnumMember]
	InvalidRequest
}
