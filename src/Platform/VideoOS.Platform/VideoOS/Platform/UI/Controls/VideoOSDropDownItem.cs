using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSDropDownItem : INotifyPropertyChanged
{
	private VideoOSIconSourceBase __iconSource;

	private object __data;

	private bool __isEnabled = true;

	private DataTemplate __contentTemplate;

	private object __tag;

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

	public object Data
	{
		get
		{
			return __data;
		}
		set
		{
			if (value != __data)
			{
				__data = value;
				OnPropertyChanged("Data");
			}
		}
	}

	public bool IsEnabled
	{
		get
		{
			return __isEnabled;
		}
		set
		{
			if (value != __isEnabled)
			{
				__isEnabled = value;
				OnPropertyChanged("IsEnabled");
			}
		}
	}

	public DataTemplate ContentTemplate
	{
		get
		{
			return __contentTemplate;
		}
		set
		{
			if (value != __contentTemplate)
			{
				__contentTemplate = value;
				OnPropertyChanged("ContentTemplate");
			}
		}
	}

	public object Tag
	{
		get
		{
			return __tag;
		}
		set
		{
			if (value != __tag)
			{
				__tag = value;
				OnPropertyChanged("Tag");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	[MethodImpl(MethodImplOptions.Synchronized)]
	public VideoOSContextMenuItem AsContextMenuItem(bool isCheckable, bool isChecked)
	{
		return new VideoOSContextMenuItem
		{
			IconSource = IconSource,
			Data = Data,
			ContentTemplate = ContentTemplate,
			IsCheckable = isCheckable,
			IsChecked = isChecked,
			IsEnabled = IsEnabled,
			StaysOpenOnClick = isCheckable,
			CheckedAppearance = (isCheckable ? VideoOSContextMenuItem.CheckedAppearances.Marked : VideoOSContextMenuItem.CheckedAppearances.Highlighted),
			Tag = this
		};
	}

	public override string ToString()
	{
		if (Data is string text && !string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return base.ToString();
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
