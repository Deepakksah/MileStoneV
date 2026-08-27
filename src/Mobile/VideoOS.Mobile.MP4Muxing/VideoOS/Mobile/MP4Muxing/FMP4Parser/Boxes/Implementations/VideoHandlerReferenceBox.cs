namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class VideoHandlerReferenceBox : HandlerReferenceBox
{
	public new const string Name = "VideoHandler";

	public VideoHandlerReferenceBox()
	{
		base.HandlerType = HandlerTypes.VideoTrack;
		base.Name = "VideoHandler";
	}
}
