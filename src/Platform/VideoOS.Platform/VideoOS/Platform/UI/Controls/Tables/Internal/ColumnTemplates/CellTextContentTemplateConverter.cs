using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Controls.Tables.Internal.ColumnTemplates;

public class CellTextContentTemplateConverter : MarkupExtension, IMultiValueConverter
{
	public DataTemplate TallSelectedTemplate { get; set; }

	public DataTemplate TallDeselectedTemplate { get; set; }

	public DataTemplate NormalSelectedTemplate { get; set; }

	public DataTemplate NormalDeselectedTemplate { get; set; }

	public DataTemplate CompactSelectedTemplate { get; set; }

	public DataTemplate CompactDeselectedTemplate { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null || values.Length < 3 || !(values[0] is VideoOSTable.RowAppearances rowAppearances) || !(values[1] is bool flag) || !(values[2] is VideoOSTable.SelectionModes selectionModes))
		{
			return null;
		}
		return rowAppearances switch
		{
			VideoOSTable.RowAppearances.Tall => (selectionModes != VideoOSTable.SelectionModes.Disabled && flag) ? TallSelectedTemplate : TallDeselectedTemplate, 
			VideoOSTable.RowAppearances.Normal => (selectionModes != VideoOSTable.SelectionModes.Disabled && flag) ? NormalSelectedTemplate : NormalDeselectedTemplate, 
			VideoOSTable.RowAppearances.Compact => (selectionModes != VideoOSTable.SelectionModes.Disabled && flag) ? CompactSelectedTemplate : CompactDeselectedTemplate, 
			_ => throw new ArgumentOutOfRangeException("rowAppearance", rowAppearances, null), 
		};
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
