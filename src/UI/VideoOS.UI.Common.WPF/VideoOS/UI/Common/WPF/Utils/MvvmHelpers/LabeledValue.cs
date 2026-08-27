using System;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class LabeledValue : Freezable, IComparable<LabeledValue>
{
	public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledValue), new PropertyMetadata(string.Empty));

	public string Label
	{
		get
		{
			return (string)GetValue(LabelProperty);
		}
		set
		{
			SetValue(LabelProperty, value);
		}
	}

	public object Value { get; set; }

	public LabeledValue()
	{
	}

	public LabeledValue(string label, object value)
	{
		Label = label;
		Value = value;
	}

	protected override Freezable CreateInstanceCore()
	{
		return new LabeledValue();
	}

	public int CompareTo(LabeledValue other)
	{
		if (other == null)
		{
			return -1;
		}
		return string.Compare(Label, other.Label, StringComparison.OrdinalIgnoreCase);
	}

	int IComparable<LabeledValue>.CompareTo(LabeledValue other)
	{
		return CompareTo(other);
	}
}
