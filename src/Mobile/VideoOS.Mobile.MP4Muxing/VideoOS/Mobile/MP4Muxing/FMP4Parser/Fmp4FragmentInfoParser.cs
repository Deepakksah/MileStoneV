using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class Fmp4FragmentInfoParser
{
	public static void Do(byte[] content, Fmp4FragmentInfo fmp4FragmentInfo)
	{
		RootBox rootBox = new RootBox();
		rootBox.InitFromBuffer(content, 0u, (uint)content.Length, BoxFactory.TimeScale);
		Fmp4TimeScaleParser fmp4TimeScaleParser = new Fmp4TimeScaleParser();
		rootBox.Traverse(new BoxVisitor(fmp4TimeScaleParser.Processors).OnBox);
		fmp4FragmentInfo.TimeScaleInFmp4 = fmp4TimeScaleParser.TimeScale;
		fmp4FragmentInfo.FragmenterState.SequenceNumber = fmp4TimeScaleParser.SequenceNumber;
	}
}
