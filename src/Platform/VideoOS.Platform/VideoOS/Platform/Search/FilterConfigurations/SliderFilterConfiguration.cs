namespace VideoOS.Platform.Search.FilterConfigurations;

public sealed class SliderFilterConfiguration : FilterConfigurationBase
{
	public double Minimum { get; set; }

	public double Maximum { get; set; } = 100.0;

	public double StepSize { get; set; } = 1.0;
}
