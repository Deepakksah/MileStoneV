using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.Util.Images;

namespace VideoOS.Platform.Search;

public abstract class SearchToolbarPluginInstance : INotifyPropertyChanged
{
	private string _title;

	private string _tooltip;

	private Icon _icon;

	private VideoOSIconSourceBase _iconSource;

	private bool _enabled = true;

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			_title = value;
			FirePropertyChanged("Title");
		}
	}

	public string Tooltip
	{
		get
		{
			return _tooltip;
		}
		set
		{
			_tooltip = value;
			FirePropertyChanged("Tooltip");
		}
	}

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public Icon Icon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value;
			FirePropertyChanged("Icon");
		}
	}

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return _iconSource ?? VideoOS.Platform.Util.Images.ImageConverter.ConvertIconToIconSourceBase(Icon);
		}
		set
		{
			_iconSource = value;
			FirePropertyChanged("IconSource");
		}
	}

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
			FirePropertyChanged("Enabled");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void FirePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public abstract ActivateResult Activate(IEnumerable<SearchResultData> searchResults);
}
