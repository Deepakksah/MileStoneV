using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("SmartClientSecurityInfo: Live={SmartClientLive}, Browse={SmartClientBrowse}, Setup={SmartClientSetup}, Report={SmartClientReport}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class SmartClientSecurityInfo
{
	[DataMember]
	public bool SmartClientLive { get; set; }

	[DataMember]
	public bool SmartClientBrowse { get; set; }

	[DataMember]
	public bool SmartClientSetup { get; set; }

	[DataMember]
	public bool SmartClientReport { get; set; }
}
