using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace VideoOS.Platform.Search.SearchFilters;

public class ColorDisplayNameComparer : IComparer<(string DisplayName, Color Value)>
{
	public int Compare((string DisplayName, Color Value) x, (string DisplayName, Color Value) y)
	{
		return string.Compare(x.DisplayName, y.DisplayName, StringComparison.CurrentCulture);
	}
}
