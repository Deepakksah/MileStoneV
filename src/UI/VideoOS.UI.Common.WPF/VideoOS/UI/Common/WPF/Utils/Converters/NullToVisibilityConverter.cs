using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class NullToVisibilityConverter : MarkupExtension, IValueConverter
{
	public Visibility NullVisibility { get; set; }

	public Visibility NotNullVisibility { get; set; }

	public NullToVisibilityConverter()
	{
		NullVisibility = Visibility.Visible;
		NotNullVisibility = Visibility.Collapsed;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value == null) ? NullVisibility : NotNullVisibility;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
