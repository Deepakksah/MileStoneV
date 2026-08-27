using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTabControlItemBase : INotifyPropertyChanged
{
	private string __text;

	private string __toolTip;

	private DataTemplate __contentTemplate;

	private object __data;

	private bool __isEnabled = true;

	private bool __isDraggable;

	private object __tag;

	private string __automationId = string.Empty;

	public string Text
	{
		get
		{
			return __text;
		}
		set
		{
			if (value != __text)
			{
				__text = value;
				OnPropertyChanged("Text");
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

	public bool IsDraggable
	{
		get
		{
			return __isDraggable;
		}
		set
		{
			if (value != __isDraggable)
			{
				__isDraggable = value;
				OnPropertyChanged("IsDraggable");
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

	public string AutomationId
	{
		get
		{
			return __automationId;
		}
		set
		{
			if (value != __automationId)
			{
				__automationId = value;
				OnPropertyChanged("AutomationId");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public VideoOSTabControlItemBase()
	{
	}

	public void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
