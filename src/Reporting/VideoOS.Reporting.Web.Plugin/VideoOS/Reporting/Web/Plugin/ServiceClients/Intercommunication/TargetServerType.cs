using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[Flags]
[DataContract(Name = "TargetServerType", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public enum TargetServerType
{
	[EnumMember]
	XPE = 1,
	[EnumMember]
	XPCO = 2
}
