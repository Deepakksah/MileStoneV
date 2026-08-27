using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.Mobile.UI.Converters;

public class EnumFlagsToBool : ValueConverterBase
{
	public int Target { get; set; }

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		Target = (int)value;
		return ((Enum)value).HasFlag((Enum)parameter);
	}

	public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is Enum)
		{
			if ((bool)value)
			{
				Target |= (int)parameter;
			}
			else
			{
				Target &= ~(int)parameter;
			}
			return Target;
		}
		return Binding.DoNothing;
	}
}
