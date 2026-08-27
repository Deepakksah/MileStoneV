using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("TimeDuration: MicroSeconds={MicroSeconds}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class TimeDuration
{
	[DataMember]
	public long MicroSeconds { get; set; }
}
