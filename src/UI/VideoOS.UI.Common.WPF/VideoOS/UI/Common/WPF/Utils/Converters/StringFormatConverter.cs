using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class StringFormatConverter : MarkupExtension, IMultiValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		object obj = ((values.Length != 0) ? values[0] : null);
		values = ((values.Length != 0) ? values.Skip(1).ToArray() : null);
		if (obj != null && obj != DependencyProperty.UnsetValue)
		{
			try
			{
				return string.Format(CultureInfo.InvariantCulture, obj.ToString(), values);
			}
			catch (FormatException)
			{
				return Binding.DoNothing;
			}
		}
		return Binding.DoNothing;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return targetTypes.Select((Type testc) => Binding.DoNothing).ToArray();
	}
}
