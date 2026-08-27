namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class ChunkOffsetBox : GenericEntriesBox<ChunkOffsetBoxEntry>
{
	public ChunkOffsetBox()
		: base("stco", (IGenericBoxEntryProcessor<ChunkOffsetBoxEntry>)new ChunkOffsetBoxEntryProcessor())
	{
	}
}
