using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTableActionsColumn : VideoOSTableColumnBase
{
	public override bool IsSortable
	{
		get
		{
			return false;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public string Header { get; set; }

	public double Spacing { get; set; } = 2.0;

	public string ItemActionsPropertyName { get; set; }

	public override string FallbackItemSortPropertyName => null;

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
			new KeyValuePair<string, string>("_ActionsPropertyName_", ItemActionsPropertyName),
			new KeyValuePair<string, string>("_Spacing_", Spacing.ToString(CultureInfo.InvariantCulture))
		};
	}
}
