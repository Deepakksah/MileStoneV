using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public class CellActionsContentTemplateConverter : MarkupExtension, IValueConverter
{
	public DataTemplate TallTemplate { get; set; }

	public DataTemplate NormalTemplate { get; set; }

	public DataTemplate CompactTemplate { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is VideoOSTable.RowAppearances rowAppearances))
		{
			return null;
		}
		return rowAppearances switch
		{
			VideoOSTable.RowAppearances.Tall => TallTemplate, 
			VideoOSTable.RowAppearances.Normal => NormalTemplate, 
			VideoOSTable.RowAppearances.Compact => CompactTemplate, 
			_ => throw new ArgumentOutOfRangeException("rowAppearance", rowAppearances, null), 
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
