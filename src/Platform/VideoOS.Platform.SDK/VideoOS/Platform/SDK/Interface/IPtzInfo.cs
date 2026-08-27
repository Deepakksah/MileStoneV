namespace VideoOS.Platform.SDK.Interface;

public interface IPtzInfo
{
	bool PtzEnabled { get; set; }

	bool EditPreset { get; set; }

	IPresetInfo[] Presets { get; set; }

	ICapabilityInfo[] Capabilities { get; set; }

	bool IsCenterOnPositionInViewSupported { get; set; }

	bool IsPtzCenterAndZoomToRectangleSupported { get; set; }

	bool IsPtzHomeSupported { get; set; }

	bool IsPtzDiagonalSupported { get; set; }
}
