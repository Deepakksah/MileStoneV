using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.MultipleSelectComboBoxControl;

public class MultipleSelectComboBoxDataContextConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return new List<IEnumerable<Item>> { value as IEnumerable<Item> };
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
