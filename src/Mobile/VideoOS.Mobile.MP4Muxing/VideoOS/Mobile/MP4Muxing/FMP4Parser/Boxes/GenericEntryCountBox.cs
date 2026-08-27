using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericEntryCountBox : FullBox
{
	public uint EntryCount;

	public override uint DataSize => 4u;

	public GenericEntryCountBox(string boxType)
	{
		base.BoxType = boxType;
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref EntryCount);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(EntryCount);
	}
}
