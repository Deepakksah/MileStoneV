using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("UserInfo: {name} ({sid})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UserInfo
{
	[DataMember]
	public string name { get; set; }

	[DataMember]
	public string sid { get; set; }
}
