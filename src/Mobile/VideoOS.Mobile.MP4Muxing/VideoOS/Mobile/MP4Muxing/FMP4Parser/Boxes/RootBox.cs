using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class RootBox : Box
{
	public override bool CanHaveChildren => true;

	public RootBox()
	{
		base.HeaderSize = 0u;
	}

	protected override uint ParseHeader(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint size, ref string type)
	{
		size = restInBoxSize;
		return 0u;
	}

	protected override void WriteHeader(Stream stream)
	{
	}
}
