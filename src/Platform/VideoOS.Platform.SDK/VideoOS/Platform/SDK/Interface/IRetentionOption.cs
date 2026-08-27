namespace VideoOS.Platform.SDK.Interface;

public interface IRetentionOption
{
	RetentionOptionType RetentionOptionType { get; set; }

	int RetentionUnits { get; set; }
}
