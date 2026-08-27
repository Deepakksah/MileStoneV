using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class PtzInfo
{
	[DataMember]
	public bool PtzEnabled { get; set; }

	[DataMember]
	public PresetInfo[] Presets { get; set; }

	[DataMember]
	public CapabilityInfo[] Capabilities { get; set; }

	[DataMember]
	public bool IsCenterOnPositionInViewSupported { get; set; }

	[DataMember]
	public bool IsPtzCenterAndZoomToRectangleSupported { get; set; }

	[DataMember]
	public bool IsPtzHomeSupported { get; set; }

	[DataMember]
	public bool IsPtzDiagonalSupported { get; set; }

	[DataMember]
	public bool EditPreset { get; set; }
}
