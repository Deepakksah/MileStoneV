using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("UpgradeInfo: {Hostname}, {Port}, {Path}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class UpgradeInfo
{
	[DataMember]
	public string Hostname { get; set; }

	[DataMember]
	public int Port { get; set; }

	[DataMember]
	public string Path { get; set; }

	[DataMember]
	public string Message { get; set; }
}
