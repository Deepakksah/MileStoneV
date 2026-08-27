namespace VideoOS.Platform.Search.FilterConfigurations;

public sealed class RegionSelectionFilterConfiguration : FilterConfigurationBase
{
	public int MaximumRegionsAllowed { get; set; } = int.MaxValue;

	public string MaximumRegionsReachedWarning { get; set; }

	public string HeaderText { get; set; }

	public string HintText { get; set; }
}
