using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Search.FilterConfigurations;

public sealed class ListSelectionFilterConfiguration : FilterConfigurationBase
{
	public string SearchBoxWatermark { get; set; }

	public Dictionary<Guid, string> Items { get; } = new Dictionary<Guid, string>();
}
