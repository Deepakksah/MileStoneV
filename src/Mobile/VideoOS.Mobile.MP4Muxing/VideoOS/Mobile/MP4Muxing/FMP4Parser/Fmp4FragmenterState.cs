namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4FragmenterState
{
	public uint SequenceNumber { get; set; }

	public void Reset()
	{
		SequenceNumber = 0u;
	}
}
