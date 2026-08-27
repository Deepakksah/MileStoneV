namespace VideoOS.Platform.Data;

public class MediaProviderSession
{
	public int Channel { get; set; }

	public object ApplicationCustomData { get; set; }

	public int FPS { get; set; }

	public MediaContent MediaContent { get; set; }

	public int ImageWidth { get; set; }

	public int ImageHeight { get; set; }

	public int Quality { get; set; }

	public int GopLength { get; set; }
}
