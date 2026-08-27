using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaLiveResult
{
	[DataMember]
	public RestrictedMediaLive[] RestrictedMedia { get; set; }

	[DataMember]
	public FaultDevice[] FaultDevices { get; set; }

	[DataMember]
	public RestrictedMediaResultStatus Status { get; set; }

	[DataMember]
	public WarningDevice[] WarningDevices { get; set; }
}
