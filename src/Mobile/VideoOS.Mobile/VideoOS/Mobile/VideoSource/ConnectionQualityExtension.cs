using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.VideoSource;

public static class ConnectionQualityExtension
{
	public static void Initialize(this ConnectionQuality quality)
	{
		quality.BandwidthDecimalInKB = 0.0;
		quality.BandwidthInKB = 0u;
		quality.FPSInTausents = 0u;
	}

	public static void Append(this ConnectionQuality quality, ConnectionQuality other)
	{
		quality.FPSInTausents += other.FPSInTausents;
		quality.BandwidthDecimalInKB += other.BandwidthDecimalInKB;
		quality.BandwidthInKB += other.BandwidthInKB;
	}
}
