using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogLevel", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public enum LogLevel
{
	[EnumMember]
	Error,
	[EnumMember]
	Warning,
	[EnumMember]
	Info,
	[EnumMember]
	Debug
}
