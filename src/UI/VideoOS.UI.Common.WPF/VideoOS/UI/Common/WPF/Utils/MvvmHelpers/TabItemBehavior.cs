using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public static class TabItemBehavior
{
	private class AlternativeSelectionBehaviour
	{
		public void Attach(TabItem tabItem)
		{
			if (tabItem.IsLoaded)
			{
				AttachEventHandlers(tabItem);
			}
			else
			{
				tabItem.Loaded += tabItem_Loaded;
			}
		}

		private void tabItem_Loaded(object sender, RoutedEventArgs e)
		{
			TabItem tabItem = sender as TabItem;
			AttachEventHandlers(tabItem);
		}

		protected virtual void AttachEventHandlers(TabItem tabItem)
		{
			tabItem.GetVisualParent<System.Windows.Controls.TabControl>().PreviewGotKeyboardFocus += tabControl_PreviewGotKeyboardFocus;
		}

		private static void tabControl_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			e.Handled = true;
		}
	}

	private class SelectOnMouseUpBehaviour : AlternativeSelectionBehaviour
	{
		protected override void AttachEventHandlers(TabItem tabItem)
		{
			base.AttachEventHandlers(tabItem);
			tabItem.MouseLeftButtonUp += tabItem_MouseLeftButtonUp;
		}

		private static void tabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			(sender as TabItem).IsSelected = true;
		}
	}

	private class SelectOnMouseDownBehaviour : AlternativeSelectionBehaviour
	{
		protected override void AttachEventHandlers(TabItem tabItem)
		{
			base.AttachEventHandlers(tabItem);
			tabItem.PreviewMouseLeftButtonDown += tabItem_PreviewMouseLeftButtonDown;
		}

		private static void tabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			(sender as TabItem).IsSelected = true;
		}
	}

	public static readonly DependencyProperty TabItemBehaviorProperty = DependencyProperty.RegisterAttached("TabItemBehavior", typeof(TabItemBehaviorOption), typeof(TabItemBehavior), new FrameworkPropertyMetadata(TabItemBehaviorOption.Default, OnTabItemBehaviorChanged));

	private static readonly DependencyProperty TabItemBehaviorImplProperty = DependencyProperty.RegisterAttached("TabItemBehaviorImpl", typeof(AlternativeSelectionBehaviour), typeof(TabItemBehavior), new FrameworkPropertyMetadata(null));

	public static TabItemBehaviorOption GetTabItemBehavior(DependencyObject dependencyObject)
	{
		return (TabItemBehaviorOption)dependencyObject.GetValue(TabItemBehaviorProperty);
	}

	public static void SetTabItemBehavior(DependencyObject dependencyObject, TabItemBehaviorOption value)
	{
		dependencyObject.SetValue(TabItemBehaviorProperty, value);
	}

	public static void SetSelectOnMouseUp(DependencyObject dependencyObject, TabItemBehaviorOption value)
	{
		dependencyObject.SetValue(TabItemBehaviorProperty, value);
	}

	public static void OnTabItemBehaviorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
	{
		if ((TabItemBehaviorOption)eventArgs.OldValue != TabItemBehaviorOption.Default)
		{
			throw new InvalidOperationException("Changing the 'TabItemBehavior.TabItemBehavior' property is currently not supported.");
		}
		if (!(dependencyObject is TabItem tabItem))
		{
			throw new InvalidOperationException("'TabItemBehavior.TabItemBehavior' property should be attached to an object of type 'TabItem'.");
		}
		TabItemBehaviorOption tabItemBehaviorOption = (TabItemBehaviorOption)eventArgs.NewValue;
		AlternativeSelectionBehaviour alternativeSelectionBehaviour = null;
		switch (tabItemBehaviorOption)
		{
		case TabItemBehaviorOption.OnMouseDown:
			alternativeSelectionBehaviour = new SelectOnMouseDownBehaviour();
			break;
		case TabItemBehaviorOption.OnMouseUp:
			alternativeSelectionBehaviour = new SelectOnMouseUpBehaviour();
			break;
		}
		tabItem.SetValue(TabItemBehaviorImplProperty, alternativeSelectionBehaviour);
		alternativeSelectionBehaviour?.Attach(tabItem);
	}
}
