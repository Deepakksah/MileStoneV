using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public partial class CellMark : ContentControl, IComponentConnector
{
	private VideoOSTable _table;

	public CellMark()
	{
		InitializeComponent();
	}

	private void CellMark_OnLoaded(object sender, RoutedEventArgs e)
	{
		_table = this.FindAncestor<VideoOSTable>();
		if (_table != null)
		{
			_table.MarkedItemsChanged += _table_MarkedItemsChanged;
			UpdateMarkedState();
		}
	}

	private void CellMark_OnUnloaded(object sender, RoutedEventArgs e)
	{
		if (_table != null)
		{
			_table.MarkedItemsChanged -= _table_MarkedItemsChanged;
		}
	}

	private void CellMark_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		UpdateMarkedState();
	}

	private void _checkBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
	{
		_checkBox.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
		{
			RoutedEvent = UIElement.MouseLeftButtonDownEvent
		});
		e.Handled = true;
	}

	private void _checkBox_OnCheckedChanged(object sender, RoutedEventArgs e)
	{
		if (base.DataContext == null)
		{
			return;
		}
		if (_checkBox.IsChecked == true)
		{
			if (!_table.MarkedItems.Contains(base.DataContext))
			{
				_table.MarkedItems.Add(base.DataContext);
			}
		}
		else if (_table.MarkedItems.Contains(base.DataContext))
		{
			_table.MarkedItems.Remove(base.DataContext);
		}
	}

	private void _table_MarkedItemsChanged(object sender, RoutedEventArgs e)
	{
		UpdateMarkedState();
	}

	private void UpdateMarkedState()
	{
		_checkBox.IsChecked = base.DataContext != null && _table != null && _table.MarkedItems.Contains(base.DataContext);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
