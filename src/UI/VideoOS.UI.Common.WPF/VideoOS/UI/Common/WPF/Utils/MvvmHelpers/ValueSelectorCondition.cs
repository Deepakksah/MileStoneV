using System.Linq;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ValueSelectorCondition : FrameworkElement
{
	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ValueSelectorCondition), new FrameworkPropertyMetadata(null, ValuePropertyChanged));

	public DependencyObject ParentInternal{ get; set; }

	public object Condition { get; set; }

	public object Value
	{
		get
		{
			return GetValue(ValueProperty);
		}
		set
		{
			SetValue(ValueProperty, value);
		}
	}

	private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		d.GetAncestors<ValueSelector>().FirstOrDefault().OnValueChanged();
	}

	protected override DependencyObject GetUIParentCore()
	{
		if (ParentInternal == null)
		{
			return base.GetUIParentCore();
		}
		return ParentInternal;
	}
}
