namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public interface IBoxFactory
{
	IBoxParse CreateBox(string boxType);
}
