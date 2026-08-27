using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class ChunkOffsetBoxEntryProcessor : IGenericBoxEntryProcessor<ChunkOffsetBoxEntry>
{
	public uint DataSize => 4u;

	public ChunkOffsetBoxEntry ParseData(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes)
	{
		ChunkOffsetBoxEntry chunkOffsetBoxEntry = new ChunkOffsetBoxEntry();
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref chunkOffsetBoxEntry.ChunkOffset);
		return chunkOffsetBoxEntry;
	}

	public void WriteToStream(Stream stream, ChunkOffsetBoxEntry entry)
	{
		stream.WriteUInt32(entry.ChunkOffset);
	}
}
