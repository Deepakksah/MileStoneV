using VideoOS.Mobile.Portable.Utilities.Rects;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class TranscodedVideoData
{
	public int Height { get; set; }

	public int Width { get; set; }

	public bool CroppingDefined { get; set; }

	public FloatRectExt CroppingRectangle { get; set; }

	public int ResamplingTag { get; set; }

	public string HardwareDecodingStatus { get; set; }

	public object CustomData { get; set; }
}
