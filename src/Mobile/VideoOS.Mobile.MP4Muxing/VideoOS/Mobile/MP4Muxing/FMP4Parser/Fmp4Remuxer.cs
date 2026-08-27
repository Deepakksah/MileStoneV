using System.IO;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4Remuxer
{
	public static byte[] Do(byte[] input, bool joinData = false)
	{
		RootBox rootBox = new RootBox();
		rootBox.InitFromBuffer(input, 0u, (uint)input.Length, BoxFactory.Fmp4Remuxer);
		if (joinData)
		{
			BoxJoinHelper.JoinMoofs(rootBox);
			BoxJoinHelper.JoinMdats(rootBox);
		}
		rootBox.Traverse(new BoxVisitor(new Fmp4Corrector().Processors).OnBox);
		rootBox.RemoveChildren((IBoxParse box) => box.BoxType == "mfra");
		byte[] array = new byte[rootBox.Size];
		rootBox.WriteToStream(new MemoryStream(array));
		return array;
	}
}
