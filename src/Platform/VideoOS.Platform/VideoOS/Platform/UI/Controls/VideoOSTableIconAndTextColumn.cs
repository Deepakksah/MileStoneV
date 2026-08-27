using System.Collections.Generic;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTableIconAndTextColumn : VideoOSTableColumnBase
{
	public string Header { get; set; }

	public string ItemIconSourcePropertyName { get; set; }

	public string ItemTextPropertyName { get; set; }

	public override string FallbackItemSortPropertyName => ItemTextPropertyName;

	public VideoOSTableIconAndTextColumn()
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
			new KeyValuePair<string, string>("_IconSourcePropertyName_", ItemIconSourcePropertyName),
			new KeyValuePair<string, string>("_TextPropertyName_", ItemTextPropertyName)
		};
	}
}
