namespace VideoOS.DataCollector.Service.Common.ResultResponse;

public readonly record struct Error
{
	public string Description { get; }

	private Error(string description)
	{
		Description = description;
	}

	public static Error Failure(string description = "A failure has occurred.")
	{
		return new Error(description);
	}
}
