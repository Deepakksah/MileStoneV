namespace VideoOS.Platform.UI.Controls;

public class VideoOSDialogActionButtonPrimary : VideoOSDialogActionButtonBase
{
	private bool __isDestructive;

	public bool IsDestructive
	{
		get
		{
			return __isDestructive;
		}
		set
		{
			if (__isDestructive != value)
			{
				__isDestructive = value;
				OnPropertyChanged("IsDestructive");
			}
		}
	}
}
