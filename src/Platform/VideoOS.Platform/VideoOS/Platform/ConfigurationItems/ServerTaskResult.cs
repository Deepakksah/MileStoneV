namespace VideoOS.Platform.ConfigurationItems;

public abstract class ServerTaskResult
{
	public ServerTask ServerTask { get; }

	public bool Success { get; set; }

	public ServerTaskResult(ServerTask serverTask)
	{
		ServerTask = serverTask;
		Success = serverTask.State == StateEnum.Success || serverTask.State == StateEnum.Completed;
	}
}
