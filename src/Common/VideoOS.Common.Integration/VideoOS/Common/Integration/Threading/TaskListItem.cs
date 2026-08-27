namespace VideoOS.Common.Integration.Threading;

public class TaskListItem
{
	public AsyncTask Worker { get; set; }

	public TaskState State { get; set; }

	public TaskListItem(AsyncTask worker, TaskState state)
	{
		Worker = worker;
		State = state;
	}
}
