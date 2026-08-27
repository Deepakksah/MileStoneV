namespace VideoOS.Platform.Search.FilterConfigurations;

public sealed class TextBoxFilterConfiguration : FilterConfigurationBase
{
	public int MaxLength { get; set; } = int.MaxValue;

	public string Watermark { get; set; } = string.Empty;
}
