using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class VisibilityToMarginConverter : MarkupExtension, IValueConverter
{
	public Thickness VisibleMargin { get; set; } = new Thickness(0.0);

	public Thickness HiddenMargin { get; set; } = new Thickness(0.0);

	public Thickness CollapsedMargin { get; set; } = new Thickness(0.0);

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		Visibility? visibility = value as Visibility?;
		if (!visibility.HasValue)
		{
			return DependencyProperty.UnsetValue;
		}
		return visibility switch
		{
			Visibility.Visible => VisibleMargin, 
			Visibility.Hidden => HiddenMargin, 
			_ => CollapsedMargin, 
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
