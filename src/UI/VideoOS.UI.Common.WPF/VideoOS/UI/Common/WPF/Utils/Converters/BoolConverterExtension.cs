using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

[MarkupExtensionReturnType(typeof(IValueConverter))]
public class BoolConverterExtension : MarkupExtension
{
	public object TrueValue { get; set; }

	public object FalseValue { get; set; }

	public bool IsOrOperation { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new BoolConverter<object>(TrueValue, FalseValue, IsOrOperation);
	}
}
