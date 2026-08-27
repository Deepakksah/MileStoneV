using System.Drawing;

namespace VideoOS.Platform.Admin;

public class MapAlarmContextMenu
{
	private Icon _icon;

	private bool _enabled = true;

	private bool _isChecked;

	private bool _isCheckable;

	public virtual Icon Icon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value;
		}
	}

	public string DisplayName { get; set; }

	public string Command { get; set; }

	public virtual bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public virtual bool IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			_isChecked = value;
		}
	}

	public virtual bool IsCheckable
	{
		get
		{
			return _isCheckable;
		}
		set
		{
			_isCheckable = value;
		}
	}

	public MapAlarmContextMenu(string displayName, string command, Icon icon)
	{
		_icon = icon;
		DisplayName = displayName;
		Command = command;
	}
}
