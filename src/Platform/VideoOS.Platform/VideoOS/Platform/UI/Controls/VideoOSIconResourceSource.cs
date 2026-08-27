namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSIconResourceSource : VideoOSIconSourceBase
{
	private string __resourceName;

	public string ResourceName
	{
		get
		{
			return __resourceName;
		}
		set
		{
			if (__resourceName != value)
			{
				__resourceName = value;
				OnPropertyChanged("ResourceName");
			}
		}
	}
}
