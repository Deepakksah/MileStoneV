using System.Collections.Generic;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTableTimestampColumn : VideoOSTableColumnBase
{
	public string Header { get; set; }

	public string ItemTimestampPropertyName { get; set; }

	public string Format { get; set; } = "G";

	public override string FallbackItemSortPropertyName => ItemTimestampPropertyName;

	public VideoOSTableTimestampColumn()
	{
		PrimarySortDirection = VideoOSTable.SortDirections.Descending;
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
			new KeyValuePair<string, string>("_TimestampPropertyName_", ItemTimestampPropertyName),
			new KeyValuePair<string, string>("_Format_", Format)
		};
	}
}
