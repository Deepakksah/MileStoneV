namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public interface IBox
{
	uint Size { get; }

	string BoxType { get; }
}
