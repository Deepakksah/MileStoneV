using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public class TimestampConverter : MarkupExtension, IMultiValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length < 2 || !(values[0] is DateTime dateTime) || !(values[1] is string text))
		{
			return string.Empty;
		}
		TimeZoneInfo destinationTimeZone = ClientControl.Instance.SelectedTimeZoneInfo ?? TimeZoneInfo.Local;
		return TimeZoneInfo.ConvertTime(dateTime, destinationTimeZone).ToString(text, CultureInfo.CurrentCulture);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
