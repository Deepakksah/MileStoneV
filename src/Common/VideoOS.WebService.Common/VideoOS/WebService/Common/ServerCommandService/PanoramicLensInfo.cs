using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class PanoramicLensInfo
{
	[DataMember]
	public bool PanoramicLensEnabled { get; set; }

	[DataMember]
	public string PanoramicLensType { get; set; }

	[DataMember]
	public string CameraMount { get; set; }

	[DataMember]
	public ImmerVisionInfo ImmerVision { get; set; }
}
