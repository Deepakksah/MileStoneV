using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class CountToDisplayStringConverter : IValueConverter
{
	public int Threshold { get; set; }

	public string DisplayStringMax { get; set; }

	public CountToDisplayStringConverter()
	{
		Threshold = 9;
		DisplayStringMax = "9";
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int num = System.Convert.ToInt32(value, CultureInfo.InvariantCulture);
		if (num > Threshold)
		{
			return DisplayStringMax;
		}
		return num.ToString(CultureInfo.InvariantCulture);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
