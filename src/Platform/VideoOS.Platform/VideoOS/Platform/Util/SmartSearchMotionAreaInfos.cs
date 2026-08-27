using System.Collections.Generic;

namespace VideoOS.Platform.Util;

public class SmartSearchMotionAreaInfos
{
	public SmartSearchSize Resolution { get; set; }

	public IEnumerable<SmartSearchMotionAreaInfo> MotionAreaInfo { get; set; }
}
