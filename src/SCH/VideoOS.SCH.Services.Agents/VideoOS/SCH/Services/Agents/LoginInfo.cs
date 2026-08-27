using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Agents;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class LoginInfo
{
	[DataMember(Order = 0)]
	public string Token { get; set; }

	[DataMember(Order = 1)]
	public DateTime RegistrationTime { get; set; }

	[DataMember(Order = 2)]
	public TimeDuration TimeToLive { get; set; }
}
