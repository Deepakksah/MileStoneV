using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public partial class HeaderMark : ContentControl, IComponentConnector
{
	public static class HeaderMarkInternals
	{
		public static IMultiValueConverter TabIndexConverter { get; } = new LambdaConverter2<VideoOSTableColumnBase, ICollection<VideoOSTableColumnBase>>((VideoOSTableColumnBase column, ICollection<VideoOSTableColumnBase> columns) => columns?.ToList().IndexOf(column), "TabIndexConverter");
	}

	private VideoOSTable _table;

	public HeaderMark()
	{
		InitializeComponent();
	}

	private void HeaderMark_OnLoaded(object sender, RoutedEventArgs e)
	{
		_table = this.FindAncestor<VideoOSTable>();
		if (_table != null)
		{
			_table.MarkedItemsChanged += _table_MarkedItemsChanged;
			_table.ItemsChanged += _table_ItemsChanged;
		}
	}

	private void HeaderMark_OnUnloaded(object sender, RoutedEventArgs e)
	{
		if (_table != null)
		{
			_table.MarkedItemsChanged -= _table_MarkedItemsChanged;
			_table.ItemsChanged -= _table_ItemsChanged;
		}
	}

	private void _checkBox_OnCheckedChanged(object sender, RoutedEventArgs e)
	{
		if (_checkBox.IsChecked == true)
		{
			if (!AreAllItemsMarked())
			{
				_table.MarkAll();
			}
		}
		else if (AreAllItemsMarked())
		{
			_table.UnmarkAll();
		}
	}

	private void _table_MarkedItemsChanged(object sender, RoutedEventArgs e)
	{
		_checkBox.IsChecked = AreAllItemsMarked();
	}

	private void _table_ItemsChanged(object sender, EventArgs e)
	{
		_checkBox.IsChecked = AreAllItemsMarked();
	}

	private bool AreAllItemsMarked()
	{
		if (_table.ItemsSource == null)
		{
			return false;
		}
		if (_table.MarkedItems.Count == 0)
		{
			return false;
		}
		List<object> list = new List<object>(_table.ItemsSource.OfType<object>());
		if (!list.Any())
		{
			return false;
		}
		if (_table.MarkedItems.Count != list.Count)
		{
			return false;
		}
		List<object> second = new List<object>(_table.MarkedItems.OfType<object>());
		return list.Intersect(second).Count() == list.Count;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
