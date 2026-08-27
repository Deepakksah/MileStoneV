namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TimeToSampleBox : GenericEntriesBox<TimeToSampleBoxEntry>
{
	public TimeToSampleBox(string boxType)
		: base(boxType, (IGenericBoxEntryProcessor<TimeToSampleBoxEntry>)new TimeToSampleBoxEntryProcessor())
	{
	}
}
