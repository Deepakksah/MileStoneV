namespace VideoOS.Platform.SDK.Interface;

public interface IPanoramicLensInfo
{
	bool PanoramicLensEnabled { get; set; }

	string PanoramicLensType { get; set; }

	string CameraMount { get; set; }

	IImmerVisionInfo ImmerVision { get; set; }
}
