using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class CountToBoolConverter : MarkupExtension, IValueConverter
{
	public int Threshold { get; set; }

	public bool EqualOrLessValue { get; set; }

	public bool GreaterThanValue { get; set; }

	public bool FallbackValue { get; set; }

	public CountToBoolConverter()
	{
		Threshold = 0;
		EqualOrLessValue = false;
		GreaterThanValue = true;
		FallbackValue = false;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if (System.Convert.ToInt32(value, CultureInfo.InvariantCulture) > Threshold)
			{
				return GreaterThanValue;
			}
			return EqualOrLessValue;
		}
		catch (InvalidCastException)
		{
			return FallbackValue;
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
