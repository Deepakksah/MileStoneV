namespace VideoOS.Common.SharedComponent;

public class ViewItemSettings
{
	public int ImageQuality { get; set; }

	public int FrameRate { get; set; }

	public int IpixSplitMode { get; set; }

	public bool MaintainImageAspectRatio { get; set; }

	public bool KeepImageQualityWhenMaximized { get; set; }

	public bool UpdateOnMotionOnly { get; set; }

	public int SoundOnMotion { get; set; }

	public int SoundOnEvent { get; set; }

	public int SmartSearchGridWidth { get; set; }

	public int SmartSearchGridHeight { get; set; }

	public string SmartSearchGridMask { get; set; }

	public ViewItemSettings()
	{
		ImageQuality = 100;
		FrameRate = 0;
		IpixSplitMode = 0;
		MaintainImageAspectRatio = true;
		KeepImageQualityWhenMaximized = false;
		UpdateOnMotionOnly = false;
		SoundOnMotion = 0;
		SoundOnEvent = 0;
		SmartSearchGridWidth = 0;
		SmartSearchGridHeight = 0;
		SmartSearchGridMask = "";
	}
}
