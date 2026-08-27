namespace VideoOS.Management.Server.TaskSecurityProviders;

public static class TaskSecurityProviderFactory
{
	public static ITaskSecurityProvider GetSecurityProvider(Task.TaskType taskType)
	{
		return TaskTypeInformation.TaskTypesDictionary[taskType].TaskSecurityProvider;
	}
}
