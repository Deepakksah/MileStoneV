using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls.Tables.Internal;

public sealed class VideoOSTableDataGrid : DataGrid
{
	public event EventHandler<NotifyCollectionChangedEventArgs> ItemsChanged;

	public void Select(IList items)
	{
		BeginUpdateSelectedItems();
		base.SelectedItems.Clear();
		foreach (object item in items)
		{
			base.SelectedItems.Add(item);
		}
		EndUpdateSelectedItems();
	}

	protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
	{
		base.OnItemsChanged(e);
		this.ItemsChanged?.Invoke(this, e);
	}
}
