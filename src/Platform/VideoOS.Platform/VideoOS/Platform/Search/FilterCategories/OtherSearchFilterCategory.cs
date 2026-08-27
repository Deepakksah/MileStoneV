using System;
using System.Collections.Generic;
using System.Drawing;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Platform.Search.FilterCategories;

public sealed class OtherSearchFilterCategory : SearchFilterCategory
{
	[Obsolete("This constructor is deprecated. Windows Forms controls are no longer supported. Use OtherSearchFilterCategory(string, VideoOSIconSourceBase, IEnumerable<SearchFilter>) instead.", false)]
	public OtherSearchFilterCategory(string name, Icon icon, IEnumerable<SearchFilter> customSearchFilters)
		: base(name, icon, customSearchFilters)
	{
	}

	public OtherSearchFilterCategory(string name, VideoOSIconSourceBase iconSource, IEnumerable<SearchFilter> customSearchFilters)
		: base(name, iconSource, customSearchFilters)
	{
	}
}
