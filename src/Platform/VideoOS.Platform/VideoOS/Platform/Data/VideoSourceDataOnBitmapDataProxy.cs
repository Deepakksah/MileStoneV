namespace VideoOS.Platform.Data;

public class VideoSourceDataOnBitmapDataProxy : SourceDataOnBitmapDataProxy, IVideoSourceData, ISourceData
{
	public string HardwareDecodingStatus
	{
		get
		{
			return base.BitmapData.HardwareDecodingStatus;
		}
		set
		{
			base.BitmapData.HardwareDecodingStatus = value;
		}
	}

	public VideoSourceDataOnBitmapDataProxy(BitmapData bitmapData)
		: base(bitmapData)
	{
	}
}
