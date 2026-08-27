using System;
using System.Windows;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

[ContentProperty("DataTemplate")]
public class DataTemplateSelectorOption : FrameworkElement
{
	public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(DataTemplateSelectorOption));

	public object Binding
	{
		get
		{
			return GetValue(BindingProperty);
		}
		set
		{
			SetValue(BindingProperty, value);
		}
	}

	public object Value { get; set; }

	public DataTemplate DataTemplate { get; set; }

	public bool DoesMatch
	{
		get
		{
			if (Value != null && Binding != null)
			{
				return string.Compare(Binding.ToString(), Value.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
			}
			return Binding == Value;
		}
	}
}
