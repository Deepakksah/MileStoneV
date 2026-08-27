using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class ClearValueOnLayoutUpdateBehavior : Behavior<UIElement>
{
	public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(ClearValueOnLayoutUpdateBehavior), new PropertyMetadata());

	public DependencyProperty Property
	{
		get
		{
			return (DependencyProperty)((DependencyObject)(object)this).GetValue(PropertyProperty);
		}
		set
		{
			((DependencyObject)(object)this).SetValue(PropertyProperty, (object)value);
		}
	}

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		if (Property != null)
		{
			base.AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
		}
	}

	private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
	{
		base.AssociatedObject.ClearValue(Property);
	}
}
