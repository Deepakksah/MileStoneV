using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.UI.Converters;

public class EnumToVideoOSIconBuiltInSourceConverter : MarkupExtension, IValueConverter
{
	public Enum MatchValue { get; set; }

	public VideoOSIconBuiltInSource.Icons MatchedResult { get; set; }

	public VideoOSIconBuiltInSource.Icons NotMatchedResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return new VideoOSIconBuiltInSource
		{
			Icon = ((value is Enum obj && obj.Equals(MatchValue)) ? MatchedResult : NotMatchedResult)
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
