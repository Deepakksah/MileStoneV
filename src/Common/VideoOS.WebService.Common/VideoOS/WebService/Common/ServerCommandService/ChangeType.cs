using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("ChangeType: Recorder={Recorder}, Hardware={Hardware}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ChangeType
{
	[DataMember]
	public bool Recorder { get; set; }

	[DataMember]
	public bool Hardware { get; set; }
}
