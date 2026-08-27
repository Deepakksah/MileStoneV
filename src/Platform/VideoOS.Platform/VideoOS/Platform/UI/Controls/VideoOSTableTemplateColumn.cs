using System.Collections.Generic;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTableTemplateColumn : VideoOSTableColumnBase
{
	public string Header { get; set; }

	public string ItemDataPropertyName { get; set; }

	public DataTemplate CellTemplate { get; set; }

	public override string FallbackItemSortPropertyName => ItemDataPropertyName;

	public VideoOSTableTemplateColumn()
	{
		PrimarySortDirection = VideoOSTable.SortDirections.Ascending;
	}

	public override List<KeyValuePair<string, string>> GetHeaderTemplateReplacePairs()
	{
		return new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("_Header_", Header)
		};
	}

	public override List<KeyValuePair<string, string>> GetCellTemplateReplacePairs()
	{
		return new List<KeyValuePair<string, string>>
		{
			new KeyValuePair<string, string>("_DataPropertyName_", ItemDataPropertyName)
		};
	}
}
