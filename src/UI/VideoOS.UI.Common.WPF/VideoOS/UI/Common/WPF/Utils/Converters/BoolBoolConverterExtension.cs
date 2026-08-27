using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

[MarkupExtensionReturnType(typeof(IValueConverter))]
public class BoolBoolConverterExtension : MarkupExtension
{
	public bool TrueValue { get; set; }

	public bool FalseValue { get; set; }

	public bool IsOrOperation { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new BoolConverter<bool>(TrueValue, FalseValue, IsOrOperation);
	}
}
