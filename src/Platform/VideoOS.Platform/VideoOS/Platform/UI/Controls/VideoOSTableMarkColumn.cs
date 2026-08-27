using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSTableMarkColumn : VideoOSTableColumnBase
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

	public override DataGridLength Width
	{
		get
		{
			return DataGridLength.Auto;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override double MinWidth
	{
		get
		{
			return -1.0;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override double MaxWidth
	{
		get
		{
			return -1.0;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override bool CanUserResize
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

	public override string ItemSortPropertyName
	{
		get
		{
			return string.Empty;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override VideoOSTable.SortDirections PrimarySortDirection
	{
		get
		{
			return VideoOSTable.SortDirections.Ascending;
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override string FallbackItemSortPropertyName => string.Empty;

	public override List<KeyValuePair<string, string>> GetHeaderTemplateReplacePairs()
	{
		return new List<KeyValuePair<string, string>>();
	}

	public override List<KeyValuePair<string, string>> GetCellTemplateReplacePairs()
	{
		return new List<KeyValuePair<string, string>>();
	}
}
