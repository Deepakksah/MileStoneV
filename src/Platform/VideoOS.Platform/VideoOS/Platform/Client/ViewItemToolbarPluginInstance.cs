using System;
using System.ComponentModel;
using System.Drawing;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.Client;

public abstract class ViewItemToolbarPluginInstance
{
	public static readonly string PropertyNameTitle = "Title";

	public static readonly string PropertyNameTooltip = "Tooltip";

	public static readonly string PropertyNameIcon = "Icon";

	public static readonly string PropertyNameIconSource = "IconSource";

	public static readonly string PropertyNameVisible = "Visible";

	public static readonly string PropertyNameEnabled = "Enabled";

	public static readonly string PropertyNameIsChecked = "IsChecked";

	public static readonly string PropertyNameIsHeld = "IsHeld";

	private string _title = string.Empty;

	private string _tooltip = string.Empty;

	private Image _icon;

	private VideoOSIconSourceBase _iconSource;

	private bool _visible = true;

	private bool _enabled = true;

	private bool _isChecked;

	private bool _isHeld;

	public string Title
	{
		get
		{
			return _title;
		}
		protected set
		{
			if (_title != value)
			{
				_title = value;
				FireToolbarPropertyChanged(PropertyNameTitle);
			}
		}
	}

	public string Tooltip
	{
		get
		{
			return _tooltip;
		}
		protected set
		{
			if (_tooltip != value)
			{
				_tooltip = value;
				FireToolbarPropertyChanged(PropertyNameTooltip);
			}
		}
	}

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use IconSource instead.", false)]
	public Image Icon
	{
		get
		{
			return _icon;
		}
		protected set
		{
			if (_icon != value)
			{
				_icon = value;
				FireToolbarPropertyChanged(PropertyNameIcon);
			}
		}
	}

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return _iconSource ?? Icon.ToIconBitmapSource();
		}
		protected set
		{
			if (_iconSource != value)
			{
				_iconSource = value;
				FireToolbarPropertyChanged(PropertyNameIconSource);
			}
		}
	}

	public bool Visible
	{
		get
		{
			return _visible;
		}
		protected set
		{
			if (_visible != value)
			{
				_visible = value;
				FireToolbarPropertyChanged(PropertyNameVisible);
			}
		}
	}

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		protected set
		{
			if (_enabled != value)
			{
				_enabled = value;
				FireToolbarPropertyChanged(PropertyNameEnabled);
			}
		}
	}

	public bool IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			if (_isChecked != value)
			{
				_isChecked = value;
				OnIsCheckedChanged();
				FireToolbarPropertyChanged(PropertyNameIsChecked);
			}
		}
	}

	public bool IsHeld
	{
		get
		{
			return _isHeld;
		}
		public set
		{
			if (_isHeld != value)
			{
				_isHeld = value;
				OnIsHeldChanged();
				FireToolbarPropertyChanged(PropertyNameIsHeld);
			}
		}
	}

	public event PropertyChangedEventHandler ToolbarPropertyChanged;

	public abstract void Init(Item viewItemInstance, Item window);

	public virtual ToolbarPluginWpfUserControl GenerateWpfUserControl()
	{
		return null;
	}

	public virtual void Activate()
	{
	}

	public virtual void OnIsCheckedChanged()
	{
	}

	public virtual void OnIsHeldChanged()
	{
	}

	public abstract void Close();

	private void FireToolbarPropertyChanged(string propertyName)
	{
		if (this.ToolbarPropertyChanged != null)
		{
			this.ToolbarPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
