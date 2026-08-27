using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TimeToSampleBoxEntryProcessor : IGenericBoxEntryProcessor<TimeToSampleBoxEntry>
{
	public uint DataSize => 8u;

	public TimeToSampleBoxEntry ParseData(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes)
	{
		TimeToSampleBoxEntry timeToSampleBoxEntry = new TimeToSampleBoxEntry();
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref timeToSampleBoxEntry.SampleCount);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref timeToSampleBoxEntry.SampleDelta);
		return timeToSampleBoxEntry;
	}

	public void WriteToStream(Stream stream, TimeToSampleBoxEntry entry)
	{
		stream.WriteUInt32(entry.SampleCount);
		stream.WriteUInt32(entry.SampleDelta);
	}
}
