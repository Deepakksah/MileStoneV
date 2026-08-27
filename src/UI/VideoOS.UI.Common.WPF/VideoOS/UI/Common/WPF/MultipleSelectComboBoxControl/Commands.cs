using System.Windows.Input;

namespace VideoOS.UI.Common.WPF.MultipleSelectComboBoxControl;

public sealed class Commands
{
	public static RoutedUICommand UpdateAllCheckBoxCommand = new RoutedUICommand("UpdateAllCheckBoxCommand", "UpdateAllCheckBoxCommand", typeof(MultipleSelectComboBoxItem));

	private Commands()
	{
	}
}
