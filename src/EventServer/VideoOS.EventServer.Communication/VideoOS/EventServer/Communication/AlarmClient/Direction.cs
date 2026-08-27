using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Direction", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public enum Direction
{
	[EnumMember]
	Ascending,
	[EnumMember]
	Descending
}
