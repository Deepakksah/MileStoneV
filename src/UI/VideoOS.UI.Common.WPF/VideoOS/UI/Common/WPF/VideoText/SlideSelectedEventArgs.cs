using System;

namespace VideoOS.UI.Common.WPF.VideoText;

public class SlideSelectedEventArgs : EventArgs
{
	public SlideType SlideType { get; private set; }

	public SlideViewModel Slide { get; private set; }

	public SlideSelectedEventArgs(SlideType type, SlideViewModel slide)
	{
		SlideType = type;
		Slide = slide;
	}
}
