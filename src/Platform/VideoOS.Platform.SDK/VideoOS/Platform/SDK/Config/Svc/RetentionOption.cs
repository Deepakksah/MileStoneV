using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config.Svc;

public sealed class RetentionOption
{
	private IRetentionOption _apiRetentionOption;

	public RetentionOptionType RetentionOptionType => _apiRetentionOption.RetentionOptionType switch
	{
		RetentionOptionType.Days => RetentionOptionType.Days, 
		RetentionOptionType.Indefinite => RetentionOptionType.Indefinite, 
		RetentionOptionType.Months => RetentionOptionType.Months, 
		RetentionOptionType.UserDefined => RetentionOptionType.UserDefined, 
		RetentionOptionType.Weeks => RetentionOptionType.Weeks, 
		RetentionOptionType.Years => RetentionOptionType.Years, 
		RetentionOptionType.Unused => RetentionOptionType.Unused, 
		_ => RetentionOptionType.Unused, 
	};

	public int RetentionUnits => _apiRetentionOption.RetentionUnits;

	public RetentionOption(IRetentionOption apiRetentionOption)
	{
		_apiRetentionOption = apiRetentionOption;
	}
}
