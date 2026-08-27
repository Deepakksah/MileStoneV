using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ButtonToolbarBehavior : Behavior<ToolBar>
{
	public DataTemplate NormalButtonTemplate { get; set; }

	public DataTemplate OverflowButtonTemplate { get; set; }

	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		Style style = new Style(typeof(ContentPresenter));
		style.Setters.Add(new Setter(ContentPresenter.ContentTemplateProperty, NormalButtonTemplate));
		Trigger trigger = new Trigger
		{
			Property = ToolBar.IsOverflowItemProperty,
			Value = true
		};
		trigger.Setters.Add(new Setter(ContentPresenter.ContentTemplateProperty, OverflowButtonTemplate));
		style.Triggers.Add(trigger);
		base.AssociatedObject.ItemContainerStyle = style;
	}
}
