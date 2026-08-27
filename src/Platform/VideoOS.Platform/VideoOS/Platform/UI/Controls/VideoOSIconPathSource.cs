using System.Windows.Shapes;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconPathSource : VideoOSIconSourceBase
{
	private Path __path;

	private bool __autoColor;

	public Path Path
	{
		get
		{
			return __path;
		}
		set
		{
			if (__path != value)
			{
				__path = value;
				OnPropertyChanged("Path");
			}
		}
	}

	public bool AutoColor
	{
		get
		{
			return __autoColor;
		}
		set
		{
			if (__autoColor != value)
			{
				__autoColor = value;
				OnPropertyChanged("AutoColor");
			}
		}
	}
}
