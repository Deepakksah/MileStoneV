using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

[MarkupExtensionReturnType(typeof(IValueConverter))]
public class BoolToVisibilityConverter : MarkupExtension
{
	public Visibility TrueValue { get; set; }

	public Visibility FalseValue { get; set; }

	public bool IsOrOperation { get; set; }

	public BoolToVisibilityConverter()
	{
		TrueValue = Visibility.Visible;
		FalseValue = Visibility.Collapsed;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new BoolConverter<Visibility>(TrueValue, FalseValue, IsOrOperation);
	}
}
