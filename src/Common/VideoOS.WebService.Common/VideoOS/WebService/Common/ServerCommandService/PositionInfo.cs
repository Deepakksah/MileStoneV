using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("PositionInfo: Pan={Pan}, Tilt={Tilt}, Zoom={Zoom}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class PositionInfo
{
	[DataMember]
	public double Pan { get; set; }

	[DataMember]
	public double Tilt { get; set; }

	[DataMember]
	public double Zoom { get; set; }
}
