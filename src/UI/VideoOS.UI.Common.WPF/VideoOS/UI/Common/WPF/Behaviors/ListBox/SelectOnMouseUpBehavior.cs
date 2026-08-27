using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Behaviors.ListBox;

public class SelectOnMouseUpBehavior : BehaviorBase<System.Windows.Controls.ListBox>
{
	private System.Windows.Controls.ListBox AssociatedObject => GetAssociatedObject() as System.Windows.Controls.ListBox;

	protected override void OnSetup()
	{
		System.Windows.Controls.ListBox associatedObject = AssociatedObject;
		if (associatedObject != null)
		{
			associatedObject.PreviewMouseDown += Element_OnPreviewMouseDown;
			associatedObject.MouseUp += Element_OnMouseUp;
		}
	}

	protected override void OnCleanup()
	{
		System.Windows.Controls.ListBox associatedObject = AssociatedObject;
		if (associatedObject != null)
		{
			associatedObject.PreviewMouseDown -= Element_OnPreviewMouseDown;
			associatedObject.MouseUp -= Element_OnMouseUp;
		}
	}

	private void Element_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void Element_OnMouseUp(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton != MouseButton.Left || !(AssociatedObject.ContainerFromElement((Visual)e.OriginalSource) is FrameworkElement frameworkElement))
		{
			return;
		}
		if (frameworkElement is ListBoxItem listBoxItem && AssociatedObject.Items.Contains(listBoxItem))
		{
			AssociatedObject.SelectedItem = listBoxItem;
			return;
		}
		object dataContext = frameworkElement.DataContext;
		if (AssociatedObject.Items.Contains(dataContext))
		{
			AssociatedObject.SelectedItem = dataContext;
		}
	}
}
