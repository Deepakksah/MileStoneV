namespace VideoOS.Platform.Search.FilterConfigurations;

public sealed class RangeSliderFilterConfiguration : FilterConfigurationBase
{
	public double Minimum { get; set; }

	public double Maximum { get; set; } = 100.0;

	public double RangeMin { get; set; }

	public double RangeMax { get; set; } = 100.0;

	public double StepSize { get; set; } = 1.0;
}
