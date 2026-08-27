using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Agents;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UserInfo
{
	[DataMember(Order = 0)]
	public string name { get; set; }

	[DataMember(Order = 1)]
	public string sid { get; set; }
}
