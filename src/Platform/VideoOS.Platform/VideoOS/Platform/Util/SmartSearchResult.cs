using System;

namespace VideoOS.Platform.Util;

public class SmartSearchResult
{
	public DateTime ImageTime { get; set; }

	public SmartSearchMask MotionMask { get; set; }

	public SmartSearchMotionAreaInfos MotionAreas { get; set; }
}
