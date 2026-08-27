using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MarkedDataResult
{
	[DataMember]
	public MarkedData MarkedData { get; set; }

	[DataMember]
	public FaultDevice[] FaultDevices { get; set; }

	[DataMember]
	public ResultStatus Status { get; set; }

	[DataMember]
	public WarningDevice[] WarningDevices { get; set; }
}
