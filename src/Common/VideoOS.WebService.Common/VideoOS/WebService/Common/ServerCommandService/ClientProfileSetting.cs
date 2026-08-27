using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("ClientProfileSetting: {Key}, {Value}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ClientProfileSetting
{
	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public string Value { get; set; }

	[DataMember]
	public bool Locked { get; set; }
}
