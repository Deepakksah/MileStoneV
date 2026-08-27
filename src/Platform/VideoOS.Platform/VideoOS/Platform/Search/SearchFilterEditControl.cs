using System;
using System.Windows.Controls;
using VideoOS.Platform.Search.FilterConfigurations;
using VideoOS.Platform.Search.FilterValues;

namespace VideoOS.Platform.Search;

public abstract class SearchFilterEditControl : UserControl
{
	public FilterValueBase FilterValue { get; public set; }

	public SearchScope SearchScope { get; public set; }

	[Obsolete("Obsolete member. Please use the FilterConfiguration.DisplayMode-property instead.")]
	public bool SnapToDefaultWidth { get; set; } = true;

	public FilterConfigurationBase FilterConfiguration { get; public set; }

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}
}
