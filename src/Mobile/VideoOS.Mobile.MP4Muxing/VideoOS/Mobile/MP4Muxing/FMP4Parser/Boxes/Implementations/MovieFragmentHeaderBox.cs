using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class MovieFragmentHeaderBox : FullBox
{
	public uint SequenceNumber;

	public override bool CanHaveData => true;

	public override uint DataSize => GetDataSize();

	public MovieFragmentHeaderBox()
	{
		base.BoxType = "mfhd";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref SequenceNumber);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(SequenceNumber);
	}

	private uint GetDataSize()
	{
		return 4u;
	}
}
