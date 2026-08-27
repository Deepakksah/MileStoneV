using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class PtzSecurityInfo
{
	[DataMember]
	public bool ManualControl { get; set; }

	[DataMember]
	public bool PresetControl { get; set; }

	[DataMember]
	public bool ReserveControl { get; set; }
}
