using System;
using VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

namespace VideoOS.UI.Common.WPF.MultipleSelectComboBoxControl;

public class Item : ViewModelBase, IComparable
{
	private bool _isSelected;

	private bool _isEnabled = true;

	private bool _isVisible = true;

	private string _text = string.Empty;

	private string _tooltip = string.Empty;

	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			SetProperty(ref _isSelected, value, () => IsSelected, "IsSelected");
		}
	}

	public bool IsEnabled
	{
		get
		{
			return _isEnabled;
		}
		set
		{
			SetProperty(ref _isEnabled, value, () => IsEnabled, "IsEnabled");
		}
	}

	public bool IsVisible
	{
		get
		{
			return _isVisible;
		}
		set
		{
			SetProperty(ref _isVisible, value, () => IsVisible, "IsVisible");
		}
	}

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			SetProperty(ref _text, value, () => Text, "Text");
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
			SetProperty(ref _tooltip, value, () => Tooltip, "Tooltip");
		}
	}

	public virtual object Tag { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is Item item)
		{
			return string.Compare(Text, item.Text, StringComparison.Ordinal) == 0;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public int CompareTo(object obj)
	{
		if (obj is Item item)
		{
			return string.Compare(Text, item.Text, StringComparison.Ordinal);
		}
		return 0;
	}
}
public class Item<T> : Item
{
	private T _data;

	public T Data
	{
		get
		{
			return _data;
		}
		set
		{
			SetProperty(ref _data, value, () => Data, "Data");
		}
	}

	public override object Tag
	{
		get
		{
			return Data;
		}
		set
		{
			Data = (T)value;
		}
	}
}
