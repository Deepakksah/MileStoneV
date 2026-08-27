namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCommandResult
{
	public bool Executed { get; private set; }

	public string ErrorMessage { get; private set; }

	public ACCommandResult(bool executed)
		: this(executed, string.Empty)
	{
	}

	public ACCommandResult(bool executed, string errorMessage)
	{
		Executed = executed;
		ErrorMessage = errorMessage;
	}
}
