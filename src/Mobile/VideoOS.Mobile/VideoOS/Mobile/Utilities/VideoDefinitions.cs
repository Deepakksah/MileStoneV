using System.Collections.Generic;
using System.Drawing;

namespace VideoOS.Mobile.Utilities;

public static class VideoDefinitions
{
	public static Dictionary<VideoType, Size> VideoSize = new Dictionary<VideoType, Size>
	{
		{
			VideoType.NoLimit,
			default(Size)
		},
		{
			VideoType.HD1080,
			new Size(1920, 1080)
		},
		{
			VideoType.HD720,
			new Size(1280, 720)
		},
		{
			VideoType.WSVGA,
			new Size(1067, 600)
		},
		{
			VideoType.WVGA,
			new Size(854, 480)
		},
		{
			VideoType.CIF,
			new Size(352, 288)
		}
	};
}
