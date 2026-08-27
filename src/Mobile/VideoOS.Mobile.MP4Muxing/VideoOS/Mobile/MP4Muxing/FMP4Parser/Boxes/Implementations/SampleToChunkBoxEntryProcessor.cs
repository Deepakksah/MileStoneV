using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class SampleToChunkBoxEntryProcessor : IGenericBoxEntryProcessor<SampleToChunkBoxEntry>
{
	public uint DataSize => 12u;

	public SampleToChunkBoxEntry ParseData(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes)
	{
		SampleToChunkBoxEntry sampleToChunkBoxEntry = new SampleToChunkBoxEntry();
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref sampleToChunkBoxEntry.FirstChunk);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref sampleToChunkBoxEntry.SamplesPerChunk);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref sampleToChunkBoxEntry.SampleDescriptionIndex);
		return sampleToChunkBoxEntry;
	}

	public void WriteToStream(Stream stream, SampleToChunkBoxEntry entry)
	{
		stream.WriteUInt32(entry.FirstChunk);
		stream.WriteUInt32(entry.SamplesPerChunk);
		stream.WriteUInt32(entry.SampleDescriptionIndex);
	}
}
