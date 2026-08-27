using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class IpixInfo
{
	[DataMember]
	public bool IpixEnabled { get; set; }

	[DataMember]
	public HemisphereInfo Hemisphere { get; set; }

	[DataMember]
	public PositionInfo Homeposition { get; set; }

	[DataMember]
	public bool CeilingMounted { get; set; }
}
