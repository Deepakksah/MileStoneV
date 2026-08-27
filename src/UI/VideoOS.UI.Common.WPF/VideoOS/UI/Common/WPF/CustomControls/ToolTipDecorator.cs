using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.CustomControls;

public class ToolTipDecorator : Decorator
{
	public override UIElement Child
	{
		get
		{
			return base.Child;
		}
		set
		{
			if (base.Child == value)
			{
				return;
			}
			base.Child = value;
			BindingExpression bindingExpression = GetBindingExpression(FrameworkElement.ToolTipProperty);
			if (base.ToolTip == null && bindingExpression == null)
			{
				DependencyProperty dependencyProperty = null;
				if (Child is TextBlock)
				{
					dependencyProperty = TextBlock.TextProperty;
				}
				else if (Child is TextBox)
				{
					dependencyProperty = TextBox.TextProperty;
				}
				else if (Child is CheckBox)
				{
					dependencyProperty = ContentControl.ContentProperty;
				}
				else if (Child is Button)
				{
					dependencyProperty = ContentControl.ContentProperty;
				}
				if (dependencyProperty != null)
				{
					SetBinding(FrameworkElement.ToolTipProperty, new Binding
					{
						Source = Child,
						Path = new PropertyPath(dependencyProperty)
					});
				}
			}
		}
	}

	protected override Size MeasureOverride(Size constraint)
	{
		Size size = base.MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
		Size size2 = base.MeasureOverride(constraint);
		ToolTipService.SetIsEnabled(this, size != size2);
		return size2;
	}
}
