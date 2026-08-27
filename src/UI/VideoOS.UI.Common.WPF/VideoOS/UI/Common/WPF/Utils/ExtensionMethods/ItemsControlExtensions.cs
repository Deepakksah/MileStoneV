using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class ItemsControlExtensions
{
	public static IEnumerable<TItemType> ExtractItems<TItemType>(this ItemsControl ctrl)
	{
		List<TItemType> list = ctrl.Items.OfType<TItemType>().ToList();
		foreach (TItemType item in list)
		{
			ctrl.Items.Remove(item);
		}
		return list;
	}
}
