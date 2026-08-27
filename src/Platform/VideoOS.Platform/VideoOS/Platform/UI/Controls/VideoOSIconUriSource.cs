using System;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconUriSource : VideoOSIconSourceBase
{
	private Uri __uri;

	public Uri Uri
	{
		get
		{
			return __uri;
		}
		set
		{
			if (__uri != value)
			{
				__uri = value;
				OnPropertyChanged("Uri");
			}
		}
	}
}
