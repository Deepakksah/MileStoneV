namespace VideoOS.Common.Integration.Command;

public class TaskSuccessResponse : TaskResponseBase
{
	public string Result { get; private set; }

	public TaskSuccessResponse()
	{
		Result = null;
	}

	public TaskSuccessResponse(string result)
	{
		Result = result;
	}
}
