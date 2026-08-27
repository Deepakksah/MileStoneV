using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("SystemMonitorStateSource: {Name} ({Id})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SystemMonitorStateSource
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string Name { get; set; }
}
