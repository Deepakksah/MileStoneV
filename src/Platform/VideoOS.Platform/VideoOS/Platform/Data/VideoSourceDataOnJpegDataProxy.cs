namespace VideoOS.Platform.Data;

public class VideoSourceDataOnJpegDataProxy : SourceDataOnJpegDataProxy, IVideoSourceData, ISourceData
{
	public string HardwareDecodingStatus
	{
		get
		{
			return base.JpegData.HardwareDecodingStatus;
		}
		set
		{
			base.JpegData.HardwareDecodingStatus = value;
		}
	}

	public VideoSourceDataOnJpegDataProxy(JPEGData jpegData)
		: base(jpegData)
	{
	}
}
