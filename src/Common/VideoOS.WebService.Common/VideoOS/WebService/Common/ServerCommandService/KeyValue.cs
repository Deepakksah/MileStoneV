using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("KeyValue: {Key}, {Value}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class KeyValue
{
	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public string Value { get; set; }
}
