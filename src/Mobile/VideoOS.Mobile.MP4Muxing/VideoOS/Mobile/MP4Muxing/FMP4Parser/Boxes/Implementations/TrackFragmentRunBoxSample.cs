using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.DataTypes;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TrackFragmentRunBoxSample
{
	public uint? SampleDuration;

	public uint? SampleSize;

	public SampleFlags SampleFlags;

	public uint? ComposTimeOffset;

	public TrackFragmentRunBoxSample()
	{
	}

	public TrackFragmentRunBoxSample(uint? sampleDuration, uint? sampleSize, uint? sampleFlags, uint? composTimeOffset)
	{
		SampleDuration = sampleDuration;
		SampleSize = sampleSize;
		ComposTimeOffset = composTimeOffset;
		if (sampleFlags.HasValue)
		{
			SampleFlags = new SampleFlags
			{
				Value = sampleFlags.Value
			};
		}
	}
}
