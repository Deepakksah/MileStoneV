using System.Collections;
using System.Windows.Forms;

namespace VideoOS.Mobile.Utilities;

public class ListViewColumnSorter : IComparer
{
	private int _columnToSort;

	private SortOrder _orderOfSort;

	private readonly NumberCaseInsensitiveComparer _objectCompare;

	private readonly ImageTextComparer _firstObjectCompare;

	public int SortColumn
	{
		get
		{
			return _columnToSort;
		}
		set
		{
			_columnToSort = value;
		}
	}

	public SortOrder Order
	{
		get
		{
			return _orderOfSort;
		}
		set
		{
			_orderOfSort = value;
		}
	}

	public ListViewColumnSorter()
	{
		_columnToSort = 0;
		_orderOfSort = SortOrder.Ascending;
		_objectCompare = new NumberCaseInsensitiveComparer();
		_firstObjectCompare = new ImageTextComparer();
	}

	public int Compare(object x, object y)
	{
		ListViewItem listViewItem = (ListViewItem)x;
		ListViewItem listViewItem2 = (ListViewItem)y;
		int num;
		if (_columnToSort == 0)
		{
			num = _firstObjectCompare.Compare(x, y);
		}
		else
		{
			ListViewItem.ListViewSubItem listViewSubItem = listViewItem.SubItems[_columnToSort];
			ListViewItem.ListViewSubItem listViewSubItem2 = listViewItem2.SubItems[_columnToSort];
			string x2 = listViewSubItem.Text;
			string y2 = listViewSubItem2.Text;
			if (listViewSubItem.Tag != null && listViewSubItem2.Tag != null)
			{
				x2 = listViewSubItem.Tag.ToString();
				y2 = listViewSubItem2.Tag.ToString();
			}
			num = _objectCompare.Compare(x2, y2);
		}
		if (_orderOfSort == SortOrder.Ascending)
		{
			return num;
		}
		if (_orderOfSort == SortOrder.Descending)
		{
			return -num;
		}
		return 0;
	}
}
