using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class HemisphereInfo
{
	[DataMember]
	public double RadiusX { get; set; }

	[DataMember]
	public double RadiusY { get; set; }

	[DataMember]
	public double CenterX { get; set; }

	[DataMember]
	public double CenterY { get; set; }
}
