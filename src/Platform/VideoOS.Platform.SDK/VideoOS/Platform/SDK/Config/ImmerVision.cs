using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public sealed class ImmerVision
{
	private IImmerVisionInfo _immerVisionInfo;

	public string LensProfileRpl => _immerVisionInfo.LensProfileRpl;

	public string LensProfileName => _immerVisionInfo.LensProfileName;

	public static string LensProfileData => string.Empty;

	public ImmerVision(IImmerVisionInfo immerVisionInfo)
	{
		_immerVisionInfo = immerVisionInfo;
	}
}
