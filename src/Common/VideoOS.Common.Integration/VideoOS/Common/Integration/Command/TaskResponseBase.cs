namespace VideoOS.Common.Integration.Command;

public class TaskResponseBase : ResponseBase
{
	public bool Async { get; set; }

	public TaskResponseBase()
	{
		Async = false;
	}
}
