using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogReaderServiceReference;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public enum LogType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	System,
	[EnumMember]
	Audit,
	[EnumMember]
	Rule
}
