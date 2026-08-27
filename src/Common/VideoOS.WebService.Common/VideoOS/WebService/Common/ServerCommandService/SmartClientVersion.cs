using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("SmartClientVersion: DisplayName={DisplayName}, Major={Major}, Minor={Minor}, Revision={Revision}, Token={Token}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SmartClientVersion
{
	[DataMember]
	public int Major { get; set; }

	[DataMember]
	public int Minor { get; set; }

	[DataMember]
	public string Revision { get; set; }

	[DataMember]
	public int Beta { get; set; }

	[DataMember]
	public bool ForceUpgrade { get; set; }

	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public UpgradeInfo UpgradeInfo { get; set; }
}
