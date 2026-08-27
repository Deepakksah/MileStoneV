using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

[MarkupExtensionReturnType(typeof(IValueConverter))]
public class ObjToTypeStrConverterMarkup : MarkupExtension
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new ObjToTypeStrConverter();
	}
}
