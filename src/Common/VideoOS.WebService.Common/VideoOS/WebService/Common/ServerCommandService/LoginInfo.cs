using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("LoginInfo: RegistrationTime={RegistrationTime}, TimeToLive={TimeToLive}, TimeToLiveLimited={TimeToLiveLimited}, Token={Token}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class LoginInfo
{
	[DataMember]
	public string Token { get; set; }

	[DataMember]
	public DateTime RegistrationTime { get; set; }

	[DataMember]
	public TimeDuration TimeToLive { get; set; }

	[DataMember]
	public bool TimeToLiveLimited { get; set; }
}
