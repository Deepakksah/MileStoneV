using System.Windows.Media.Imaging;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconBitmapSource : VideoOSIconSourceBase
{
	private BitmapSource __bitmapSource;

	public BitmapSource BitmapSource
	{
		get
		{
			return __bitmapSource;
		}
		set
		{
			if (__bitmapSource != value)
			{
				__bitmapSource = value;
				OnPropertyChanged("BitmapSource");
			}
		}
	}
}
