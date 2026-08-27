using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSAlert : INotifyPropertyChanged
{
	private int __count = (int)VideoOSBadgeBase.CountProperty.GetMetadata(typeof(VideoOSBadgeBase)).DefaultValue;

	private int __maxDisplayCount = (int)VideoOSBadgeBase.MaxDisplayCountProperty.GetMetadata(typeof(VideoOSBadgeBase)).DefaultValue;

	private string __toolTip = (string)FrameworkElement.ToolTipProperty.GetMetadata(typeof(VideoOSBadgeBase)).DefaultValue;

	public int Count
	{
		get
		{
			return __count;
		}
		set
		{
			if (value != __count)
			{
				__count = value;
				OnPropertyChanged("Count");
			}
		}
	}

	public int MaxDisplayCount
	{
		get
		{
			return __maxDisplayCount;
		}
		set
		{
			if (value != __maxDisplayCount)
			{
				__maxDisplayCount = value;
				OnPropertyChanged("MaxDisplayCount");
			}
		}
	}

	public string ToolTip
	{
		get
		{
			return __toolTip;
		}
		set
		{
			if (value != __toolTip)
			{
				__toolTip = value;
				OnPropertyChanged("ToolTip");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
