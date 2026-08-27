using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class SelectTextOnFocus : Behavior<FrameworkElement>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		base.AssociatedObject.IsKeyboardFocusWithinChanged += AssociatedObject_IsKeyboardFocusWithinChanged;
	}

	private void AssociatedObject_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		((DispatcherObject)(object)this).Dispatcher.BeginInvoke(delegate
		{
			if (base.AssociatedObject is TextBox textBox)
			{
				textBox.SelectAll();
			}
			else
			{
				base.AssociatedObject.GetChildren<TextBox>().ForEach(delegate(TextBox tb)
				{
					tb.SelectAll();
				});
			}
		});
	}
}
