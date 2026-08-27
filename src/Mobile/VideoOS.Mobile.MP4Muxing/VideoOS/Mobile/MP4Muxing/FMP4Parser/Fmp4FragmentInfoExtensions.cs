using System;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class Fmp4FragmentInfoExtensions
{
	public static bool IsInitialized(this IFmp4FragmentInfo fmp4FragmentInfo)
	{
		return DateTime.MinValue != fmp4FragmentInfo.FirstFrameTimeUtc;
	}
}
