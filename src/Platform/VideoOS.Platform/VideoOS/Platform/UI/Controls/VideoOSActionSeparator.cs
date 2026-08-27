using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionSeparator : VideoOSActionBase
{
	[MethodImpl(MethodImplOptions.Synchronized)]
	public override object AsContextMenuItem()
	{
		Separator separator = new Separator();
		separator.SetBinding(UIElement.VisibilityProperty, new Binding("IsVisible")
		{
			Mode = BindingMode.OneWay,
			Source = this,
			Converter = new BoolToVisibilityConverter()
		});
		return separator;
	}
}
