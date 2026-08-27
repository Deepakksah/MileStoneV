using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public sealed class PanoramicLens
{
	private IPanoramicLensInfo _panoramicLensInfo;

	public bool PanoramicLensEnabled
	{
		get
		{
			if (_panoramicLensInfo == null)
			{
				return false;
			}
			return _panoramicLensInfo.PanoramicLensEnabled;
		}
	}

	public PanoramicLensType PanoramicLensType
	{
		get
		{
			if (_panoramicLensInfo == null)
			{
				return PanoramicLensType.Undefined;
			}
			if (_panoramicLensInfo.PanoramicLensType == "immervision")
			{
				return PanoramicLensType.ImmerVision;
			}
			return PanoramicLensType.Undefined;
		}
	}

	public CameraMountType CameraMount
	{
		get
		{
			if (_panoramicLensInfo == null)
			{
				return CameraMountType.Undefined;
			}
			return _panoramicLensInfo.CameraMount switch
			{
				"ceiling" => CameraMountType.Ceiling, 
				"wall" => CameraMountType.Wall, 
				"ground" => CameraMountType.Ground, 
				_ => CameraMountType.Undefined, 
			};
		}
	}

	public ImmerVision ImmerVision
	{
		get
		{
			if (_panoramicLensInfo == null || _panoramicLensInfo.ImmerVision == null)
			{
				return null;
			}
			return new ImmerVision(_panoramicLensInfo.ImmerVision);
		}
	}

	public PanoramicLens(IPanoramicLensInfo panoramicLensInfo)
	{
		_panoramicLensInfo = panoramicLensInfo;
	}
}
