using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

[MarkupExtensionReturnType(typeof(bool))]
public class StringToEnabledConverter : MarkupExtension, IValueConverter
{
	public bool EnableWhenEmpty { get; set; }

	public StringToEnabledConverter()
	{
		EnableWhenEmpty = false;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if (string.IsNullOrEmpty(System.Convert.ToString(value, CultureInfo.InvariantCulture)))
			{
				return EnableWhenEmpty;
			}
			return true;
		}
		catch (InvalidCastException)
		{
			return DependencyProperty.UnsetValue;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
