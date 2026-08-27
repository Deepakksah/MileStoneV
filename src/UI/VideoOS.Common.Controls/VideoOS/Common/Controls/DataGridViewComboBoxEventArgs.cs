using System;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class DataGridViewComboBoxEventArgs : EventArgs
{
	public int ColumnIndex { get; private set; }

	public int RowIndex { get; private set; }

	public DataGridViewComboBoxEditingControl EditingControl { get; private set; }

	public DataGridViewComboBoxEventArgs(int columnIndex, int rowIndex, DataGridViewComboBoxEditingControl editingControl)
	{
		ColumnIndex = columnIndex;
		RowIndex = rowIndex;
		EditingControl = editingControl;
	}
}
