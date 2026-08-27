using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class ImmerVisionInfo
{
	[DataMember]
	public string LensProfileRpl { get; set; }

	[DataMember]
	public string LensProfileName { get; set; }

	[DataMember]
	public string LensProfileData { get; set; }

	[DataMember]
	public double LensProfileFieldOfView { get; set; }
}
