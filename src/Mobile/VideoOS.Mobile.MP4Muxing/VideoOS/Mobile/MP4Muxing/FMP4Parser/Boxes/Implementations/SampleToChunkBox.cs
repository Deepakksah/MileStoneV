namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class SampleToChunkBox : GenericEntriesBox<SampleToChunkBoxEntry>
{
	public SampleToChunkBox()
		: base("stsc", (IGenericBoxEntryProcessor<SampleToChunkBoxEntry>)new SampleToChunkBoxEntryProcessor())
	{
	}
}
