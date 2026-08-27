using System.Collections;
using System.Windows.Forms;

namespace VideoOS.Mobile.Utilities;

public class ImageTextComparer : IComparer
{
	private readonly NumberCaseInsensitiveComparer _objectCompare;

	public ImageTextComparer()
	{
		_objectCompare = new NumberCaseInsensitiveComparer();
	}

	public int Compare(object x, object y)
	{
		ListViewItem listViewItem = (ListViewItem)x;
		int imageIndex = listViewItem.ImageIndex;
		ListViewItem listViewItem2 = (ListViewItem)y;
		int imageIndex2 = listViewItem2.ImageIndex;
		if (imageIndex < imageIndex2)
		{
			return -1;
		}
		if (imageIndex == imageIndex2)
		{
			return _objectCompare.Compare(listViewItem.Text, listViewItem2.Text);
		}
		return 1;
	}
}
