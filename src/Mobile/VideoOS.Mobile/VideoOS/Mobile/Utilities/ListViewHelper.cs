using System.Windows.Forms;

namespace VideoOS.Mobile.Utilities;

public class ListViewHelper
{
	public static string GetSelectionText(ListView listView)
	{
		string result = "";
		int[] array = new int[1];
		listView.SelectedIndices.CopyTo(array, 0);
		if (array[0] >= 0 && array[0] < listView.Items.Count)
		{
			result = listView.Items[array[0]].Text;
		}
		return result;
	}

	public static bool SetSelection(ListView listView, string text)
	{
		bool result = false;
		int i;
		for (i = 0; i < listView.Items.Count && !(listView.Items[i].Text == text); i++)
		{
		}
		if (i < listView.Items.Count)
		{
			listView.SelectedIndices.Add(i);
			result = true;
		}
		return result;
	}
}
