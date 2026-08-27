using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTreeViewItem : INotifyPropertyChanged
{
	private VideoOSIconSourceBase __iconSource;

	private object __data;

	private bool __isEnabled = true;

	private bool __isSelected;

	private bool __isExpanded;

	private ICollection<VideoOSTreeViewItem> __children;

	private DataTemplate __contentTemplate;

	private ICollection<VideoOSActionBase> __actions;

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

	public bool IsSelected
	{
		get
		{
			return __isSelected;
		}
		set
		{
			if (value != __isSelected)
			{
				__isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}
	}

	public bool IsExpanded
	{
		get
		{
			return __isExpanded;
		}
		set
		{
			if (value != __isExpanded)
			{
				__isExpanded = value;
				OnPropertyChanged("IsExpanded");
			}
		}
	}

	public ICollection<VideoOSTreeViewItem> Children
	{
		get
		{
			return __children;
		}
		set
		{
			if (!object.Equals(value, __children))
			{
				__children = value;
				OnPropertyChanged("Children");
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

	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
