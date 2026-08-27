using System;
using System.ComponentModel;

namespace VideoOS.Platform.Client;

public abstract class WorkSpaceToolbarPluginGroup : PropertyClass
{
	public static readonly string PropertyNameTitle = "Title";

	public static readonly string PropertyNameTooltip = "Tooltip";

	private string _title = string.Empty;

	private string _tooltip = string.Empty;

	public abstract Guid Id { get; }

	public virtual Guid? ParentGroupId { get; }

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
				FirePropertyChanged(PropertyNameTitle);
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
				FirePropertyChanged(PropertyNameTooltip);
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	private void FirePropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
