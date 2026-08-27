using System.Collections.Generic;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTabControlPrimaryItem : VideoOSTabControlItemBase
{
	private VideoOSIconSourceBase __iconSource;

	private ICollection<VideoOSActionBase> __actions;

	private VideoOSAlert __alert;

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return __iconSource;
		}
		set
		{
			if (value != __iconSource)
			{
				__iconSource = value;
				OnPropertyChanged("IconSource");
			}
		}
	}

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return __actions;
		}
		set
		{
			if (value != __actions)
			{
				__actions = value;
				OnPropertyChanged("Actions");
			}
		}
	}

	public VideoOSAlert Alert
	{
		get
		{
			return __alert;
		}
		set
		{
			if (value != __alert)
			{
				__alert = value;
				OnPropertyChanged("Alert");
			}
		}
	}
}
