using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.UI.Converters;

public class BoolToVideoOSIconBuiltInSourceConverter : MarkupExtension, IValueConverter
{
	public VideoOSIconBuiltInSource.Icons TrueResult { get; set; }

	public VideoOSIconBuiltInSource.Icons FalseResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = value != null && (bool)value;
		return new VideoOSIconBuiltInSource
		{
			Icon = (flag ? TrueResult : FalseResult)
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
