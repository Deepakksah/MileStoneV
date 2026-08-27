using System.Windows.Forms;

namespace VideoOS.Mobile.Utilities;

public class ListViewColumnSorterExt : ListViewColumnSorter
{
	public void OnColumnSelected(int column)
	{
		if (column == base.SortColumn)
		{
			if (base.Order == SortOrder.Ascending)
			{
				base.Order = SortOrder.Descending;
			}
			else
			{
				base.Order = SortOrder.Ascending;
			}
		}
		else
		{
			base.SortColumn = column;
			base.Order = SortOrder.Ascending;
		}
	}
}
